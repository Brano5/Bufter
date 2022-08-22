//Modal
const deleteModal = document.getElementById('deleteModal')
deleteModal.addEventListener('show.bs.modal', event => {
    const button = event.relatedTarget
    deleteModal.querySelector('.modal-body p').textContent = `Delete Room ${button.getAttribute('data-bs-name')}?`
    deleteModal.querySelector('.modal-footer #Id').value = button.getAttribute('data-bs-id')
})

const editModal = document.getElementById('editModal')
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
    editModal.querySelector('.modal-footer #Id').value = button.getAttribute('data-bs-id')
})

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