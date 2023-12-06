let deleteBtn = document.querySelectorAll(".remove-basket-item")

deleteBtn.forEach(btn => btn.addEventListener("click", function (e) {
    e.preventDefault();

    Swal.fire({
        title: "Are you sure?",
        text: "it will not revert again?",
        icon: "warning",
        showCancelButton: true,
        confirmButtonColor: "#3085d6",
        cancelButtonColor: "#d33",
        confirmButtonText: "Delete!"
    }).then((result) => {
        if (result.isConfirmed) {
            let url = btn.getAttribute("href")
            fetch(url)
                .then(response => {
                    if (response.status == 200) {
                        Swal.fire({
                            title: "it was deleted",
                            text: "Your file has been deleted.",
                            icon: "success"
                        });
                        btn.parentElement.parentElement.remove()
                    }
                    else {
                        Swal.fire({
                            title: "Deleted!",
                            text: "Deleted",
                            icon: "error"
                        });
                    }
                })
        }
    });
}))