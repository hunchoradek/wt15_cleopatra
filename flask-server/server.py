from flask import Flask, send_file
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

API_BASE_URL = "https://localhost:7057/api"

def fetch_data_from_api(endpoint):
    response = requests.get(f"{API_BASE_URL}/{endpoint}", verify=False)
    if response.status_code == 200:
        return response.json()
    else:
        return []

@app.route('/')
def hello_world():
    return 'Hello, World!'

@app.route('/generate_report')
def generate_report():
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