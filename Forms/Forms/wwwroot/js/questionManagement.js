

function initializeQuestionManagement() {
    initialQuestions.forEach(function (question, index) {
        let questionElement = createQuestionForm(question.Title, question.Description);

        document.getElementById("sortable").appendChild(questionElement);
        $.validator.setDefaults({ ignore: [] });
        $.validator.unobtrusive.parse(`#form${index}`);
    });

    let forms = document.querySelector('#sortable').querySelectorAll("form");
    for (i = 0; i < forms.length; i++) {
        $(forms[i]).on('keydown', function (e) {
            if (e.key === "Enter" &&
                ($(e.target)[0] != $("textarea")[0])) {
                e.preventDefault(); // Prevent form submission
                return false;
            }
        });
    }
    checkCreateQuestionButton();
}

$(function () {
    let sortable = $('#sortable');
    sortable.sortable({
        update: function (event, ui) { sortableUpdated(event, ui); }
    });

});


function createQuestionForm(title = "", description = "") {
    let index = $("#sortable li").length;

    // Clone the template
    let template = document.getElementById("questionTemplate");
    let clone = document.importNode(template.content, true);

    let li = clone.querySelector("li");
    li.id = `qli${index}`;
    li.id = `qli${index}`;
    li.setAttribute("data", index);

    // Populate fields
    let form = clone.querySelector("form");
    form.id = `form${index}`;
    form.setAttribute("data", initialQuestions[index].Id);

    let select = form.querySelector("select");
    select.id = "selectQType" + index;
    select.name = `selectQType[${index}]`;
    select.value = initialQuestions[index].Type;
    $(select).find("option:first").prop("disabled", true);



    $(select).selectmenu();
    $(select).selectmenu("option", "disabled", true);
    $(select).selectmenu({
        open: function (event, ui) { onQTypeDropdownOpen(event, ui) },
        select: function (event, ui) { onQTypeDropdownChosen(event, ui) },
        close: function (event) { resetSelectedDDVal(event.target.value, event.target); }
    });

    $(select).next('.ui-selectmenu-button').on('mousedown', function (e) {        
        e.stopImmediatePropagation(); // Prevents dragging behavior from triggering
    });

    let inputs = form.querySelectorAll("input");
    inputs[0].id = `title${index}`;
    inputs[0].name = `QuestionList[${index}].Title`;
    inputs[0].value = title;

    let textAreas = clone.querySelectorAll("textarea");
    textAreas[0].id = `description${index}`;
    textAreas[0].name = `QuestionList[${index}].Description`;
    textAreas[0].value = description;

    let spans = form.querySelectorAll(".templateValidation");
    spans[0].setAttribute("data-valmsg-for", `selectQType[${index}]`);
    spans[1].setAttribute("data-valmsg-for", `QuestionList[${index}].Title`);    

    let buttons = form.querySelectorAll("button");
    buttons.forEach(x => x.setAttribute("data", index));

    return clone;
}

let prevOpenDropdown = null;
function onQTypeDropdownOpen(event, ui) {    

    let $select = $(event.target);   

    // Close the currently open dropdown if it's not the same as this one
    if (prevOpenDropdown && prevOpenDropdown !== $select[0]) {
        console.log('close because');
        $(prevOpenDropdown).selectmenu("close");
    }
    prevOpenDropdown = $select[0]

    $select.children("option").each(function (index, option) {
        if (index == 0) return;
        let qType = questionTypes[index - 1];
        option.text = `${qType.DisplayName} (${qType.Left})`;
        $(option).prop("disabled", qType.Left <= 0);

    });
    $select.selectmenu("refresh");
}

function onQTypeDropdownChosen(event, ui) {
    let id = $(ui)[0].item.value;
    if (!id || isNaN(id)) return;

    let freedId = event.target.getAttribute("data");
    event.target.setAttribute("data", id);
    countQuestionTypeData(freedId, id);
}

function resetSelectedDDVal(id, select) {

    let qType = questionTypes.find(obj => {
        return obj.Id == id;
    });
    if (qType && id != '') {      
        let option = $(select).find(`option[value="${id}"]`);
        $(option).text(qType.DisplayName);
        $(option).prop("disabled", false);
    }    
    $(select).selectmenu("refresh");   
    $(select).valid();    
}



function countQuestionTypeData(freedId, chosenId) {
    if (freedId === chosenId)
        return;
    if (!isNaN(freedId) && freedId != '') {
        let questionType = questionTypes.find(x => x.Id == freedId);
        questionType.Left += 1;
    }
    questionType = questionTypes.find(x => x.Id == chosenId);
    questionType.Left -= 1;
}

function onSaveQuestion(args) {
    let id = args.getAttribute('data');
    let form = $(`#form${id}`);
    let data = form.serializeArray();
    console.log("form " + JSON.stringify(data));

    if (!form.valid())
        return;

    let questionData = initialQuestions.find(x => x.Id == form.attr("data"));
    let questionClone = { ...questionData };
    questionClone.Type = form.find("select").val();
    questionClone.Title = data[0].value;
    questionClone.Description = data[1].value;

    //console.log("data 0: " + data[0].value);
    //console.log("data 1: " + data[1].value);


    let link = saveQuestionLink;
    fetch(link, {
        method: 'POST',
        headers: getAjaxHeaders(),
        body: JSON.stringify(questionClone)
    }).then(response => response.json())
        .then(data => {
            if (data.success) {
                questionData = questionClone;
                alert('saved');
            }
            else {
                alert(data.message);
            }
        })
        .catch(error => {
            alert('Something went wrong');
        });
}

function onDeleteQuestion(args) {
    let id = args.getAttribute('data');
    let form = $(`#form${id}`);
    let questionData = initialQuestions.find(x => x.Id == form.attr("data"));

    let li = form.parent("li");

    let link = deleteQuestionLink;
    fetch(link, {
        method: 'POST',
        headers: getAjaxHeaders(),
        body: JSON.stringify(questionData)
    }).then(response => response.json())
        .then(data => {
            if (data.success) {
                questionTypes.find(x => x.Id == questionData.Type).Left += 1;
                initialQuestions.splice(initialQuestions.indexOf(questionData), 1);
                $(li).remove();
                checkCreateQuestionButton();
            }
            else {
                alert(data.message);
            }
        })
        .catch(error => {
            alert('Something went wrong');
        });
}

function checkCreateQuestionButton() {
    let questionButton = document.getElementById("CreateQButton");
    let isHidden = initialQuestions.length >= maxQuestionCount;
    if (isHidden) {
        questionButton.setAttribute('hidden', true);
    }
    else {
        questionButton.removeAttribute('hidden');
    }
}

function sortableUpdated(event, ui) {

    let sortable = $("#sortable");
    let idsInOrder = sortable.sortable("toArray");
    console.log(event.target.id);

    let reorderInfoArr = [];
    idsInOrder.forEach((value, index) => {
        let qId = sortable.find(`#${value}`).find("form").attr("data");
        reorderInfoArr.push({ "QId": qId, orderIndex: index });
    });
    
    
    let link = changeOrderLink;
    fetch(link, {
        method: 'POST',
        headers: getAjaxHeaders(),
        body: JSON.stringify(reorderInfoArr)
    }).then(response => response.json())
        .then(data => {

        })
        .catch(error => {
            alert('Something went wrong');
        });
}

function getAjaxHeaders() {
    let antiforgeryName = "__RequestVerificationToken";
    let antiForgeryVal = $("#form0").find(`input[name=${antiforgeryName}]`).val();
    let headers = new Headers();
    headers.append("Content-Type", "application/json");
    headers.append(antiForgeryKey, antiForgeryVal);
    return headers;
}




