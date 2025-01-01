let timeout;
const searchInput = document.getElementById("userSearch");
const resultsUl = document.getElementById("userResults");
const userResultsDiv = document.getElementById('userResultsCont');

searchInput.addEventListener("input", () => {
    clearTimeout(timeout);
    timeout = setTimeout(() => {
        const query = searchInput.value.trim();
        console.log("search for: " + templateId);

        if (query.length > 0) {
            fetch(`TemplateManagement?handler=SearchUsers&query=${encodeURIComponent(query)}&templateId=${templateId}`)
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

    fetch(`TemplateManagement?handler=AddUserToTemplate&templateId=${templateId}`, {
        method: "POST",
        headers: getAjaxHeaders(),
        body: JSON.stringify(user),
    }).then(response => response.json())
        .then(data => {
            console.log(JSON.stringify(data));
            if (data.success) {
                addUserToUI(user);
                alert("user added");
            }
            else {
                alert(data.message);
            }
        })
        .catch(error => {
            alert('Something went wrong');
        });
}

function addUserToUI(user) {
    let template = document.getElementById("uatempl");
    let clone = document.importNode(template.content, true);

    let span = clone.querySelector("span");
    span.innerText = user.email;

    let mainDiv = clone.querySelector("div");
    mainDiv.setAttribute('data', user.email);
    mainDiv.setAttribute('data-id', user.userId);

    let list = document.getElementById("listUWCMT");
    list.appendChild(clone);

    const searchInput = document.getElementById("userSearch");
    searchInput.value = "";
}

function onDeleteFromTemplate(target) {
    let list = document.getElementById("listUWCMT");
    let element = target.closest("div");
    let email = element.getAttribute("data");
    let id = element.getAttribute("data-id");

    let link = `TemplateManagement?handler=DeleteUserFromTemplate&templateId=${templateId}`;
    fetch(link, {
        method: 'POST',
        headers: getAjaxHeaders(),
        body: JSON.stringify({ userId: id, email: email })
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

function onSetPrivacy(isPublic) {
    let link = `TemplateManagement?handler=ChangePrivacy&isPublic=${isPublic}&templateId=${templateId}`
    fetch(link, {
        method: 'POST',
        headers: getAjaxHeaders()
    }).then(response => response.json())
        .then(data => {
            console.log(JSON.stringify(data));
            if (data.success) {                
                alert('privacy changed');
                setPrivacyUIVisibility(isPublic);
            }
            else {
                alert(data.message);
            }
        })
        .catch(error => {
            alert('Something went wrong');
        });
}

function setPrivacyUIVisibility(isPublic) {
    let makePublicUI = document.getElementById('tummpuui');
    let makePrivateUI = document.getElementById('tummprui');
    let privacyUI = document.getElementById('tumui');

    if (isPublic) {        
        makePublicUI.setAttribute("hidden", true);
        makePrivateUI.removeAttribute("hidden");
        privacyUI.setAttribute("hidden", true);
    }
    else {
        makePublicUI.removeAttribute("hidden");
        privacyUI.removeAttribute("hidden");
        makePrivateUI.setAttribute("hidden", true);
    }
}