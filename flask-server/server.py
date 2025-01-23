from flask import Flask, send_file, render_template, request, redirect, url_for, make_response, session
from flask_login import LoginManager, UserMixin, login_user, logout_user, login_required, current_user
from reportlab.lib.pagesizes import A4
from reportlab.pdfgen import canvas
from reportlab.pdfbase.ttfonts import TTFont
from reportlab.pdfbase import pdfmetrics
from reportlab.lib.styles import getSampleStyleSheet, ParagraphStyle
from reportlab.platypus import Paragraph
from datetime import datetime
import requests
import io
import urllib3
import os

urllib3.disable_warnings(urllib3.exceptions.InsecureRequestWarning)

app = Flask(__name__)
app.secret_key = "supersecretkey"

API_BASE_URL = "https://localhost:7057"

# Flask-Login Setup
login_manager = LoginManager()
login_manager.init_app(app)
login_manager.login_view = "login"

# Simulated User class for Flask-Login
class User(UserMixin):
    def __init__(self, user_id, username, role):
        self.id = user_id
        self.username = username
        self.role = role

@login_manager.user_loader
def load_user(user_id):
    # Here, you'd typically fetch the user from your database or API
    session_data = session.get("user")
    if session_data and str(session_data["id"]) == user_id:
        return User(session_data["id"], session_data["username"], session_data["role"])
    return None

def fetch_data_from_api(endpoint):
    response = requests.get(f"{API_BASE_URL}/api{endpoint}", verify=False)
    if response.status_code == 200:
        return response.json()
    else:
        return []

@app.route('/')
def hello_world():
    return 'Hello, World!'

@app.route('/login', methods=['GET', 'POST'])
def login():
    if request.method == 'POST':
        username = request.form['username']
        password = request.form['password']
        
        print(f"Logging in with username: {username} and password: {password}")
        
        # Wywołanie API do logowania
        response = requests.post(f"{API_BASE_URL}/auth/Login", json={
            "username": username,
            "password": password
        }, verify=False)
        
        print(f"API response status code: {response.status_code}")
        print(f"API response content: {response.content}")
        
        if response.status_code == 200:
            user_data = response.json()
            print(f"user_data: {user_data}")  # Dodajemy to, aby zobaczyć zawartość user_data
            user = User(user_data["id"], user_data["username"], user_data["role"])
            login_user(user)
            session["user"] = {"id": user.id, "username": user.username, "role": user.role}
            return redirect(url_for("generate_report"))
        else:
            return "Invalid credentials", 401

    return '''
    <form method="post">
        Username: <input type="text" name="username"><br>
        Password: <input type="password" name="password"><br>
        <input type="submit" value="Login">
    </form>
    '''

@app.route('/logout')
@login_required
def logout():
    logout_user()
    session.pop("user", None)
    return redirect(url_for("login"))


@app.route('/generate_report')
@login_required
def generate_report():
    if current_user.role != "manager":
        return "Access denied: Only managers can generate reports.", 403
    
    clients = fetch_data_from_api("Clients")
    appointments = fetch_data_from_api("Appointments")
    services = fetch_data_from_api("Services")
    resources = fetch_data_from_api("Resources")
    notifications = fetch_data_from_api("Notifications")

    clients_list = clients.get('$values', [])
    appointments_list = appointments.get('$values', [])
    services_list = services.get('$values', [])
    resources_list = resources.get('$values', [])
    notifications_list = notifications.get('$values', [])

    pdf_buffer = io.BytesIO()
    pdf = canvas.Canvas(pdf_buffer, pagesize=A4)
    width, height = A4

    font_path = os.path.join(os.path.dirname(os.path.abspath(__file__)), 'DejaVuSans.ttf')
    font_path_bold = os.path.join(os.path.dirname(os.path.abspath(__file__)), 'DejaVuSans-Bold.ttf')
    pdfmetrics.registerFont(TTFont('DejaVuSans', font_path))
    pdfmetrics.registerFont(TTFont('DejaVuSans-Bold', font_path_bold))

    pdf.setTitle("Raport salonu kosmetycznego")
    pdf.setFont("DejaVuSans-Bold", 16)
    pdf.drawString(200, height - 50, "Raport salonu kosmetycznego")
    pdf.setFont("DejaVuSans", 12)
    pdf.drawString(50, height - 80, f"Data generowania raportu: {datetime.now().strftime('%Y-%m-%d %H:%M:%S')}")

    y_position = height - 120

    styles = getSampleStyleSheet()
    styleN = ParagraphStyle(name='Normal', parent=styles['Normal'], fontName='DejaVuSans', fontSize=10)
    styleB = ParagraphStyle(name='Heading2', parent=styles['Heading2'], fontName='DejaVuSans-Bold', fontSize=14)

    pdf.setFont("DejaVuSans-Bold", 14)
    pdf.drawString(50, y_position, "Lista klientów")
    y_position -= 20
    pdf.setFont("DejaVuSans", 10)
    y_position -= 20
    for client in clients_list:
        text = f"ID: {client['client_id']} | Imię: {client['name']} | Telefon: {client['phone_number']} | Email: {client['email']} | Notatki: {client.get('notes', '')}"
        paragraph = Paragraph(text, styleN)
        paragraph.wrapOn(pdf, width - 100, height)
        paragraph.drawOn(pdf, 50, y_position)
        y_position -= 15 + paragraph.height
        if y_position < 50:
            pdf.showPage()
            y_position = height - 50

    pdf.setFont("DejaVuSans-Bold", 14)
    pdf.drawString(50, y_position, "Harmonogram rezerwacji")
    y_position -= 20
    pdf.setFont("DejaVuSans", 10)
    y_position -= 20
    for appointment in appointments_list:
        text = f"Data: {appointment['appointment_date']} | Start: {appointment['start_time']} | Klient ID: {appointment['client_id']} | Pracownik: {appointment['employee_name']} | Usługa: {appointment['service']} | Status: {appointment['status']}"
        paragraph = Paragraph(text, styleN)
        paragraph.wrapOn(pdf, width - 100, height)
        paragraph.drawOn(pdf, 50, y_position)
        y_position -= 15 + paragraph.height
        if y_position < 50:
            pdf.showPage()
            y_position = height - 50

    pdf.setFont("DejaVuSans-Bold", 14)
    pdf.drawString(50, y_position, "Lista usług")
    y_position -= 20
    pdf.setFont("DejaVuSans", 10)
    y_position -= 20
    for service in services_list:
        text = f"Nazwa: {service['name']} | Opis: {service.get('description', '')} | Czas trwania: {service['duration']} min | Cena: {service['price']} PLN"
        paragraph = Paragraph(text, styleN)
        paragraph.wrapOn(pdf, width - 100, height)
        paragraph.drawOn(pdf, 50, y_position)
        y_position -= 15 + paragraph.height
        if y_position < 50:
            pdf.showPage()
            y_position = height - 50

    pdf.setFont("DejaVuSans-Bold", 14)
    pdf.drawString(50, y_position, "Stan zasobów")
    y_position -= 20
    pdf.setFont("DejaVuSans", 10)
    y_position -= 20
    for resource in resources_list:
        text = f"Nazwa: {resource['name']} | Ilość: {resource['quantity']} {resource['unit']} | Poziom uzupełniania: {resource['reorder_level']}"
        paragraph = Paragraph(text, styleN)
        paragraph.wrapOn(pdf, width - 100, height)
        paragraph.drawOn(pdf, 50, y_position)
        y_position -= 15 + paragraph.height
        if y_position < 50:
            pdf.showPage()
            y_position = height - 50

    pdf.setFont("DejaVuSans-Bold", 14)
    pdf.drawString(50, y_position, "Powiadomienia")
    y_position -= 20
    pdf.setFont("DejaVuSans", 10)
    y_position -= 20
    for notification in notifications_list:
        text = f"Typ: {notification['type']} | Treść: {notification['content']} | Wysłano: {notification['sent_at']}"
        paragraph = Paragraph(text, styleN)
        paragraph.wrapOn(pdf, width - 100, height)
        paragraph.drawOn(pdf, 50, y_position)
        y_position -= 15 + paragraph.height
        if y_position < 50:
            pdf.showPage()
            y_position = height - 50

    pdf.save()
    pdf_buffer.seek(0)

    return send_file(pdf_buffer, as_attachment=True, download_name="raport_salon.pdf", mimetype="application/pdf")

if __name__ == '__main__':
    app.run(debug=True)