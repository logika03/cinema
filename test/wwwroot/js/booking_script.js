$(function () {
    window.pricePerSeat = Number($('#pricePerSeat').val());
    window.totalPrice = 0;
    window.$price = $('#price');
    window.seatsPerRow = Number($('#seatsPerRow').val());
    window.scheduleId = Number($('#scheduleId').val());

    $('.seat-button').each(function (index) {
        let $this = $(this);
        let val = GetGridElementsPosition(index);
        $this.attr("row", val.row);
        $this.attr("col", val.column);
        $this.text(val.column + 1);

        let rows = $('.places-grid');
        let row = undefined;
        if (rows.length > val.row)
            row = rows[val.row];
        else {
            row = $(document.createElement("div"));
            row.addClass('places-grid');
            row.appendTo($('#places-grid'));
        }

        $this.appendTo(row);
    });

    $('#places').remove();

    $('.seat-button:not([disabled])').click(function () {
        let $this = $(this);
        if ($this.hasClass('chosen')) {
            window.totalPrice -= window.pricePerSeat;
            window.$price.text(window.totalPrice);
            $this.removeClass('chosen');
        }
        else {
            window.totalPrice += window.pricePerSeat;
            window.$price.text(window.totalPrice);
            $this.addClass('chosen');
        }
    });

    $('#book-submit').click(function () {
        $('#book-submit').prop("disabled", true);
        let seats = [];
        $('.seat-button:not([disabled]).chosen').each(function () {
            let $this = $(this);
            let place = {
                row: Number($this.attr("row")), 
                seat: Number($this.attr("col"))
            };
            seats.push(place);
        })

        $.ajax({
            type: "POST",
            url: window.location.origin + `/booking/${window.scheduleId}/book_seats`,
            headers: { "RequestVerificationToken": $('input[name="__RequestVerificationToken"]').val() },
            data: JSON.stringify(seats),
            contentType: 'application/json',
            dataType: 'text'
        }).done(function () {
            document.location.reload();
        })
            .fail(function () {
                $('#booking-alert').removeClass('d-none');
                $('#book-submit').prop("disabled", false);
            });
    });
});

function GetGridElementsPosition(index) {
    const colCount = window.seatsPerRow;
    const rowPosition = Math.floor(index / colCount);
    const colPosition = index % colCount;

    return { row: rowPosition, column: colPosition };
}

