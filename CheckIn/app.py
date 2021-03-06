from flask import Flask, request, session, redirect, url_for, render_template

from logic.handler import CheckinCommandHandler
from logic.view import CheckinRoomsAvailabilityView

app = Flask(__name__)
app.secret_key = b'_5#y2L"F4Q8z\n\xec]/'


def show_checkin_page(booking_id):
    view = CheckinRoomsAvailabilityView(booking_id=booking_id)
    try:
        context = view.check()
    except Exception as e:
        return show_exception({'message': str(e)})
    else:
        return render_template("checkin.html", **context)


def show_exception(exception):
    return render_template("exception.html", **{'exception': exception})


@app.route('/')
def index():
    return redirect(url_for('checkin'))


@app.route('/checkin', methods=['GET', 'POST'])
def checkin():
    if request.method == 'POST':
        room_id = request.args.get('room')
        booking_id = request.args.get('booking')

        if room_id is None:
            return show_exception({'message': 'Room ID is missing'})
        if booking_id is None:
            return show_exception({'message': 'Booking ID is missing'})

        handler = CheckinCommandHandler(room_id=room_id, booking_id=booking_id)
        try:
            result = handler.process()
        except Exception as e:
            return show_exception({'message': str(e)})
        else:
            return result

    else:
        booking_id = request.args.get('booking')
        if booking_id is None:
            return show_exception({'message': 'Booking ID is missing'})

        return show_checkin_page(booking_id)
