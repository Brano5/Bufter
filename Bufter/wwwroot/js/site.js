//Modal
const deleteModal = document.getElementById('deleteModal')
if (deleteModal != null) {
    deleteModal.addEventListener('show.bs.modal', event => {
        const button = event.relatedTarget
        deleteModal.querySelector('.modal-body p').textContent = `Delete ${button.getAttribute('data-bs-name')}?`
        deleteModal.querySelector('.modal-footer #IdDeleteModal').value = button.getAttribute('data-bs-id')
    })
}

const editModal = document.getElementById('editModal')
if (editModal != null) {
    editModal.addEventListener('show.bs.modal', event => {
        const button = event.relatedTarget
        const image = button.getAttribute('data-bs-image')
        editModal.querySelector('.modal-body #NameEditModal').value = button.getAttribute('data-bs-name')
        if (button.getAttribute('data-bs-description') != null) {
            editModal.querySelector('.modal-body #DescriptionEditModal').value = button.getAttribute('data-bs-description')
        }
        if (button.getAttribute('data-bs-roomId') != null) {
            editModal.querySelector('.modal-body #RoomIdEditModal').value = button.getAttribute('data-bs-roomId')
        }
        if (button.getAttribute('data-bs-bill') != null) {
            editModal.querySelector('.modal-body #BillEditModal').value = button.getAttribute('data-bs-bill')
        }
        if (button.getAttribute('data-bs-totalBill') != null) {
            editModal.querySelector('.modal-body #TotalBillEditModal').value = button.getAttribute('data-bs-totalBill')
        }
        if (button.getAttribute('data-bs-count') != null) {
            editModal.querySelector('.modal-body #CountEditModal').value = button.getAttribute('data-bs-count')
        }
        if (button.getAttribute('data-bs-price') != null) {
            editModal.querySelector('.modal-body #PriceEditModal').value = button.getAttribute('data-bs-price')
        }
        editModal.querySelector('.modal-footer #IdEditModal').value = button.getAttribute('data-bs-id')
    })
}

const addMoneyModal = document.getElementById('addMoneyModal')
if (addMoneyModal != null) {
    addMoneyModal.addEventListener('show.bs.modal', event => {
        const button = event.relatedTarget

        var a = addMoneyModal.querySelectorAll('.modal-body a').forEach(element => element.setAttribute("href", element.getAttribute("href") + '&Person=' + button.getAttribute('data-bs-name')));
    })
}

//Popovers from bootstrap
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

//Auto close alert
function hide(id) {
    setTimeout(function () {
        $('#' + id).alert('close');
    }, 5000);
}

//Alert auto dismis
//const alerts = document.getElementsByClassName('alert-dismissible')
//if (alerts != null) {
//    alerts.each(function (i, obj) {
//        hide(obj.id);
//    });
//}


//$('.changeActiveClass').click(function (e) {

//    $('.changeActiveClass').removeClass('active');

//    var $this = $(this);
//    if (!$this.hasClass('active')) {
//        $this.addClass('active');
//    }
//});


//Graf Plotly
function chartPlotly(x, y) {
    //Data
    var data = [{
        x: x,
        y: y,
        type: 'scatter'
    }];

    //Layout
    var layout = {
        xaxis: { autorange: true, title: "Dátum" },
        yaxis: { autorange: true, title: "Počet" },
        title: "Počet nákupov"
    };

    //Display Plotly
    Plotly.newPlot("myPlot", data, layout);
}

//form validation from bootstrap
(function () {
    'use strict'

    // Fetch all the forms we want to apply custom Bootstrap validation styles to
    var forms = document.querySelectorAll('.needs-validation')

    // Loop over them and prevent submission
    Array.prototype.slice.call(forms)
        .forEach(function (form) {
            form.addEventListener('submit', function (event) {
                if (!form.checkValidity()) {
                    event.preventDefault()
                    event.stopPropagation()
                }

                form.classList.add('was-validated')
            }, false)
        })
})()


//autocomplete search via AJAX
function searchHint(search, db) {
    searchHintPos();
    searchHintClear();
    if (search.length == 0) {
        return;
    } else {
        var xmlhttp = new XMLHttpRequest();
        xmlhttp.onreadystatechange = function () {
            if (this.readyState == 4 && this.status == 200) {
                if (this.responseText != "null") {
                    var a = this.responseText.slice(1, -1).split(',');
                    for (var i = 0; i < a.length; i++) {
                        var div = document.getElementById("searchHintDiv");
                        var button = document.createElement("button");
                        button.type = "button";
                        button.className = "list-group-item list-group-item-action";
                        //button.onclick = "setPersonSearch(this.value)";
                        button.setAttribute("onclick", "setSearchValue(this.textContent);");
                        button.textContent = a[i];

                        div.appendChild(button);
                    }
                } else {
                    var div = document.getElementById("searchHintDiv");
                    var button = document.createElement("button");
                    button.type = "button";
                    button.className = "list-group-item list-group-item-action";
                    button.disabled = true;
                    button.textContent = "No match";

                    div.appendChild(button);
                }
            }
        };
        xmlhttp.open("GET", "../../Manage/" + db + "SearchHint?RoomId=" + document.getElementById("RoomId").value + "&Search=" + search, true);
        xmlhttp.send();
    }
}

function searchHintClear() {
    var div = document.getElementById("searchHintDiv");
    if (div != null) {
        var child = div.lastElementChild;
        while (child) {
            div.removeChild(child);
            child = div.lastElementChild;
        }
    }
}

function searchHintClearT() {
    setTimeout(function () { 
    var div = document.getElementById("searchHintDiv");
    if (div != null) {
        var child = div.lastElementChild;
        while (child) {
            div.removeChild(child);
            child = div.lastElementChild;
        }
        }
    }, 100);
}

function searchHintPos() {
    var input = document.getElementById("Search");
    var div = document.getElementById("searchHintDiv");
    if (input != null && div != null) {
        div.style.width = input.offsetWidth + "px";
        div.style.left = input.offsetLeft + "px";
        div.style.top = input.offsetTop + input.offsetHeight + "px";
    }
}

function setSearchValue(value) {
    var input = document.getElementById("Search");
    if (input != null) {
        input.value = value;
        input.parentElement.submit();
    }
}

window.onresize = function (event) {
    searchHintClear();
};


// in table editing via AJAX
function showEdit(a) {
    $(a).closest("tr").find(".editP").hide();
    $(a).closest("tr").find(".editInput").show();

    $(a).closest("tr").find(".editA").hide();
    $(a).closest("tr").find(".removeA").hide();
    $(a).closest("tr").find(".cancelA").show();
    $(a).closest("tr").find(".saveA").show();
}

function hideEdit(a) {
    $(a).closest("tr").find(".editP").show();
    $(a).closest("tr").find(".editInput").hide();

    $(a).closest("tr").find(".editA").show();
    $(a).closest("tr").find(".removeA").show();
    $(a).closest("tr").find(".cancelA").hide();
    $(a).closest("tr").find(".saveA").hide();
}

function saveEdit(a) {
    var id = a.closest("tr").firstChild.textContent;
    var name = a.closest("tr").getElementsByClassName("editInput name")[0].value;
    var password = a.closest("tr").getElementsByClassName("editInput password")[0].value;

    var xmlhttp = new XMLHttpRequest();
    xmlhttp.onreadystatechange = function () {
        if (this.readyState == 4 && this.status == 200) {
            var b = this.responseText.slice(1, -1).split(',');
            if (b[0] == "warning") {
                alert(b[1], 'warning');
            } else {
                hideEdit(a);
                a.closest("tr").getElementsByClassName("editP name")[0].textContent = b[1];
                a.closest("tr").getElementsByClassName("editP password")[0].textContent = b[2];
                a.closest("tr").getElementsByClassName("editTd updated")[0].textContent = b[3];
                alert('successfully edited!', 'success');
            }
        }
    };
    xmlhttp.open("POST", "../../User/Edit?Id=" + id + "&Name=" + name + "&Password=" + password, true);
    xmlhttp.send();
}

