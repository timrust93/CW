﻿let timeout;
const searchInput = document.getElementById("userSearch");
const resultsUl = document.getElementById("userResults");
const userResultsDiv = document.getElementById('userResultsCont');

searchInput.addEventListener("input", () => {
    clearTimeout(timeout);
    timeout = setTimeout(() => {
        const query = searchInput.value.trim();

        if (query.length > 0) {
            fetch(`SearchTest?handler=SearchUsers&query=${encodeURIComponent(query)}`)
                .then((response) => response.json())
                .then((data) => {
                    resultsUl.innerHTML = "";

                    userResultsDiv.removeAttribute('hidden');
                    if (data.length === 0) {
                        let li = document.createElement("li");
                        li.innerText = "-- No results found --";
                        resultsUl.appendChild(li);
                    }
                    else {
                        data.forEach((user) => {
                            let li = document.createElement("li");
                            li.innerText = user.email;
                            li.classList.add("searchResLine");
                            const email = user.email;
                            const regex = new RegExp(`(${query})`, "i"); // Case-insensitive match
                            const highlightedEmail = email.replace(regex, `<span style="background-color: yellow;">$1</span>`);
                            li.innerHTML = highlightedEmail; // Use innerHTML to apply the highlighted HTML
                            li.onclick = () => addUserToTemplate(user);
                            resultsUl.appendChild(li);
                        });
                    }

                });
        }
        else {
            resultsUl.innerHTML = "";
            userResultsDiv.setAttribute("hidden", true);
        }
    }, 300); // Debounce delay
});

function addUserToTemplate(user) {
    const userResultsDiv = document.getElementById('userResultsCont');
    userResultsDiv.setAttribute("hidden", true);
    //return;

    fetch(`SearchTest?handler=AddUserToTemplate`, {
        method: "POST",
        headers: { "Content-Type": "application/json" },
        body: JSON.stringify({ templateId: 2, "userId": user.id }),
    }).then((response) => {
        if (response.ok) {
            addUserToUI(user);
            alert("User added!");
        }
    });
}

function addUserToUI(user) {
    console.log("add user to ui: " + JSON.stringify(user));
    let template = document.getElementById("uatempl");
    let clone = document.importNode(template.content, true);

    let span = clone.querySelector("span");
    span.innerText = user.email;

    let mainDiv = clone.querySelector("div");
    mainDiv.setAttribute('data', user.id);

    let list = document.getElementById("listUWCMT");
    list.appendChild(clone);

    const searchInput = document.getElementById("userSearch");
    searchInput.value = "";
}

function onDeleteFromTemplate(target) {
    let list = document.getElementById("listUWCMT");
    let element = target.closest("div");
    let data = element.getAttribute("data");
    //console.log("data: " + data);

    let link = `SearchTest?handler=DeleteUserFromTemplate`
    fetch(link, {
        method: 'POST',
        headers: { "Content-Type": "application/json" },
        body: JSON.stringify({ templateId: 2, "userId": data })
    }).then(response => response.json())
        .then(data => {
            if (data.success) {
                list.removeChild(element);
                alert('user deleted');
            }
            else {
                alert(data.message);
            }
        })
        .catch(error => {
            alert('Something went wrong');
        });
}