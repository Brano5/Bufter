const deleteModal = document.getElementById('deleteModal')
deleteModal.addEventListener('show.bs.modal', event => {
    const button = event.relatedTarget
    const id = button.getAttribute('data-bs-id')
    const name = button.getAttribute('data-bs-name')
    const modalBodyP = deleteModal.querySelector('.modal-body p')
    const modalFooterA = deleteModal.querySelector('.modal-footer a')

    modalBodyP.textContent = `Delete Room ${name}?`
    modalFooterA.href = `${modalFooterA.href}\\${id}`
})