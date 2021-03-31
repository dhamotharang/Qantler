(function (self) {

    $('.select-multiple').select2();

    $('.select-single').select2({
        placeholder: 'Select',
        allowClear: true
    });

    self.toParams = function () {
        var customerCode = $('#filterCustomerCode').val();
        var customerName = $('#filterCustomerName').val();
        var premise = $('#filterPremise').val();
        var postal = $('#filterPostal').val();
        var status = $('#filterStatusSelect').select2('data');
        var serialNo = $('#filterSerialNo').val();
       

        function appendParam(p, key, val) {
            if (p.length > 0) {
                p += '&';
            }
            p += key + '=' + val;
            return p;
        }

        var params = '';
        if (status.length > 0) {
            status.forEach((e, i) => {
                params = appendParam(params, 'status[' + i + ']', e.id);
            });
        }        

        if (customerCode.trim().length > 0) {
            params = appendParam(params, 'customerCode', customerCode.trim());
        }

        if (customerName.trim().length > 0) {
            params = appendParam(params, 'customerName', customerName.trim());
        }

        if (premise.trim().length > 0) {
            params = appendParam(params, 'premise', premise.trim());
        }

        if (postal.trim().length > 0) {
            params = appendParam(params, 'postal', postal.trim());
        }

        if (serialNo.trim().length > 0) {
            params = appendParam(params, 'serialNo', serialNo.trim());
        }

        var issuedOnFrom = $('#datepicker-issuedOnFrom').datepicker('getDate');
        if (issuedOnFrom) {
            params = appendParam(params, 'issuedOnFrom', (issuedOnFrom ? moment(issuedOnFrom).startOf('day').utc().format() : ''));
        } 

        var issuedOnTo = $('#datepicker-issuedOnTo').datepicker('getDate');
        if (issuedOnTo) {
            params = appendParam(params, 'issuedOnTo', (issuedOnTo ? moment(issuedOnTo).startOf('day').utc().format() : ''));
        }

        return params;
    };

    function hide() {
        $('#filterCloseButton').click();
    }

    function clear() {
        $('#filterCustomerCode').val('');
        $('#filterCustomerName').val('');
        $('#filterPremise').val('');
        $('#filterPostal').val('');
        $('#filterSerialNo').val('');
        $('#filterStatusSelect').val(null).trigger('change');       
        $('#datepicker-issuedOnFrom').datepicker('update', null);
        $('#datepicker-issuedOnTo').datepicker('update', null);  
    }

    self.reset = function () {
        clear();

        $('#filterStatusSelect').val(app.page.model.defaultStatuses);
        $('#filterStatusSelect').trigger('change');
        
    }
   

    $(function () {
        $('.datepicker').datepicker({
            enableOnReadonly: true,
            todayHighlight: true,
            container: '#filterModal'
        });

        $('.datepicker').on('dp.show', function () {
            var datepicker = $('body').find('.bootstrap-datetimepicker-widget:last');
            if (datepicker.hasClass('bottom')) {
                var top = $(this).offset().top + $(this).outerHeight();
                var left = $(this).offset().left;
                datepicker.css({
                    'top': top + 'px',
                    'bottom': 'auto',
                    'left': left + 'px'
                });
            } else if (datepicker.hasClass('top')) {
                var top = $(this).offset().top - datepicker.outerHeight();
                var left = $(this).offset().left;
                datepicker.css({
                    'top': top + 'px',
                    'bottom': 'auto',
                    'left': left + 'px'
                });
            }
        });

        $('#filterApplyButton').click(function () {
            if (app.page.fetch()) {
                hide();
            }
        });

        $("#filterClearButton").click(function () {
            clear();
        });

        $('#filterResetButton').click(function () {
            self.reset();
        });

      
    });

})(app.page.filter = app.page.filter || {});

$(document).ready(function () {
    app.page.filter.reset();
});