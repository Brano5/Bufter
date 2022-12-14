//Modal
const deleteModal = document.getElementById('deleteModal')
if (deleteModal != null) {
    deleteModal.addEventListener('show.bs.modal', event => {
        const button = event.relatedTarget
        deleteModal.querySelector('.modal-body p').textContent = `Delete ${button.getAttribute('data-bs-name')}?`
        deleteModal.querySelector('.modal-footer #Id').value = button.getAttribute('data-bs-id')
    })
}

const editModal = document.getElementById('editModal')
if (editModal != null) {
    editModal.addEventListener('show.bs.modal', event => {
        const button = event.relatedTarget
        const image = button.getAttribute('data-bs-image')
        editModal.querySelector('.modal-body #Name').value = button.getAttribute('data-bs-name')
        if (button.getAttribute('data-bs-description') != null) {
            editModal.querySelector('.modal-body #Description').value = button.getAttribute('data-bs-description')
        }
        if (button.getAttribute('data-bs-roomId') != null) {
            editModal.querySelector('.modal-body #RoomId').value = button.getAttribute('data-bs-roomId')
        }
        if (button.getAttribute('data-bs-bill') != null) {
            editModal.querySelector('.modal-body #Bill').value = button.getAttribute('data-bs-bill')
        }
        if (button.getAttribute('data-bs-totalBill') != null) {
            editModal.querySelector('.modal-body #TotalBill').value = button.getAttribute('data-bs-totalBill')
        }
        if (button.getAttribute('data-bs-count') != null) {
            editModal.querySelector('.modal-body #Count').value = button.getAttribute('data-bs-count')
        }
        if (button.getAttribute('data-bs-price') != null) {
            editModal.querySelector('.modal-body #Price').value = button.getAttribute('data-bs-price')
        }
        editModal.querySelector('.modal-footer #Id').value = button.getAttribute('data-bs-id')
    })
}

const addMoneyModal = document.getElementById('addMoneyModal')
if (addMoneyModal != null) {
    addMoneyModal.addEventListener('show.bs.modal', event => {
        const button = event.relatedTarget

        var a = addMoneyModal.querySelectorAll('.modal-body a').forEach(element => element.setAttribute("href", element.getAttribute("href") + '&Person=' + button.getAttribute('data-bs-name')));
    })
}

//Popovers
const popoverTriggerList = document.querySelectorAll('[data-bs-toggle="popover"]')
const popoverList = [...popoverTriggerList].map(popoverTriggerEl => new bootstrap.Popover(popoverTriggerEl))

//Alert
const alertPlaceholder = document.getElementById('alertPlaceholder')

function alert(message, type) {
    const wrapper = document.createElement('div');

    var id = Math.floor(Math.random() * 1000000);

    switch (type) {
        case "info":
            wrapper.innerHTML = [
                '<div class="alert alert-primary alert-dismissible d-flex align-items-center fade show" role="alert" id="', id, '">',
                '<svg class="bi flex-shrink-0 me-2" width="24" height="24" role="img" aria-label="Info:"><use xlink:href="#info-fill" /></svg>',
                '<div>', message, '</div>',
                '<button type="button" class="btn-close" data-bs-dismiss="alert" aria-label="Close"></button>',
                '</div>'
            ].join('');
            break;
        case "success":
            wrapper.innerHTML = [
                '<div class="alert alert-success alert-dismissible d-flex align-items-center fade show" role="alert" id="', id, '">',
                '<svg class="bi flex-shrink-0 me-2" width="24" height="24" role="img" aria-label="Success:"><use xlink:href="#check-circle-fill" /></svg>',
                '<div>', message, '</div>',
                '<button type="button" class="btn-close" data-bs-dismiss="alert" aria-label="Close"></button>',
                '</div>'
            ].join('');
            break;
        case "warning":
            wrapper.innerHTML = [
                '<div class="alert alert-warning alert-dismissible d-flex align-items-center fade show" role="alert" id="', id, '">',
                '<svg class="bi flex-shrink-0 me-2" width="24" height="24" role="img" aria-label="Warning:"><use xlink:href="#exclamation-triangle-fill" /></svg>',
                '<div>', message, '</div>',
                '<button type="button" class="btn-close" data-bs-dismiss="alert" aria-label="Close"></button>',
                '</div>'
            ].join('');
            break;
    }

    alertPlaceholder.append(wrapper);

    hide(id);
}


$('.changeActiveClass').click(function (e) {

    $('.changeActiveClass').removeClass('active');

    var $this = $(this);
    if (!$this.hasClass('active')) {
        $this.addClass('active');
    }
});



var xArray = [50, 60, 70, 80, 90, 100, 110, 120, 130, 140, 150];
var yArray = [7, 8, 8, 9, 9, 9, 10, 11, 14, 14, 15];

// Define Data
var data = [{
    x: xArray,
    y: yArray,
    mode: "lines"
}];

// Define Layout
var layout = {
    xaxis: { range: [40, 160], title: "Square Meters" },
    yaxis: { range: [5, 16], title: "Price in Millions" },
    title: "House Prices vs Size"
};

// Display using Plotly
Plotly.newPlot("myPlot", data, layout);