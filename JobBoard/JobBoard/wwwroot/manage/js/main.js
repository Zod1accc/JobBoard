
//company activate

let itemCompanyActive = document.querySelectorAll(".comopanyactive")

itemCompanyActive.forEach(btn => btn.addEventListener("click", function (e) {
    e.preventDefault();

    Swal.fire({
        title: 'Are you sure?',
        text: "You won't be able to revert this!",
        icon: 'warning',
        showCancelButton: true,
        confirmButtonColor: '#3085d6',
        cancelButtonColor: '#d33',
        confirmButtonText: 'Yes!'
    }).then((result) => {
        if (result.isConfirmed) {
            let url = btn.getAttribute("href");
            fetch(url)
                .then(res => {
                    if (res.status == 200) {
                        alert("Activ olundu.")
                        window.location.reload(true);
                    }
                    else {
                        alert("Item tapilmadi")
                    }
                })
        }
    })
}))

//company deactivate


let itemCompanyDeactive = document.querySelectorAll(".comopanydeactive")

itemCompanyDeactive.forEach(btn => btn.addEventListener("click", function (e) {
    e.preventDefault();

    Swal.fire({
        title: 'Are you sure?',
        text: "You won't be able to revert this!",
        icon: 'warning',
        showCancelButton: true,
        confirmButtonColor: '#3085d6',
        cancelButtonColor: '#d33',
        confirmButtonText: 'Yes!'
    }).then((result) => {
        if (result.isConfirmed) {
            let url = btn.getAttribute("href");
            fetch(url)
                .then(res => {
                    if (res.status == 200) {
                        alert("Deactiv olundu.")
                        window.location.reload(true)
                    }
                    else {
                        alert("Item tapilmadi")
                    }
                })
        }
    })
}))


//job save

let saveJobBtn = document.querySelectorAll(".save-job-btn")

saveJobBtn.forEach(btn => btn.addEventListener("click", function (e) {
    e.preventDefault();
    let url = btn.getAttribute("href");
    fetch(url)
        .then(response => response.json())
        .then(res => {
            if (res.status == 200) {
                alert("Elave Olundu")
                //window.location.reload(true)
            }
            else if (res.status == 201) {
                alert("Silindi")
            }
            else {
                alert("Item tapilmadi")
            }
        })
}))














//header imaage

//const form = document.queryselector('#headerımage');
//const submitbtn = document.queryselector("#changebuttn");

//submitbtn.addeventlistener('click', (event) => {
//    event.preventdefault(); 

//    const formdata = new formdata(form);

//    fetch('/admin/Header/HeaderImage', {
//        method: 'post',
//        body: formdata
//    })
//        .then(response => response.json())
//        .then(data => console.log("ok"))
//        .catch(error => console.error(error));
//});



//const form = document.querySelector('#headerImage');
//const input = document.querySelector('#myInput');
//const btn = document.querySelector('#changeButtn');

//function handleSubmit(event) {
//    event.preventDefault();
//    const url = '/admin/Header/HeaderImage';
//    const data = { input: input.value };

//    fetch(url, {
//        method: 'POST',
//        headers: {
//            'Content-Type': 'application/json'
//        },
//        body: JSON.stringify(data)
//    })
//        .then(response => {
//            if (!response.ok) {
//                throw new Error('Network response was not ok');
//            }
//            return response.json();
//        })
//        .then(data => {
//            console.log(data);
//            // Do something with the response data
//        })
//        .catch(error => {
//            console.error('Error:', error);
//        });
//}

//form.addEventListener('submit', handleSubmit);