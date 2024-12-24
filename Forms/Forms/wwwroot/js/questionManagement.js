

function initializeQuestionManagement() {
    initialQuestions.forEach(function (question, index) {
        console.log("initial questions: " + index);
        var questionElement = createQuestionForm(question.Title, question.Description);

        document.getElementById("sortable").appendChild(questionElement);
        $.validator.setDefaults({ ignore: [] });
        $.validator.unobtrusive.parse(`#form${index}`);
    });

    var forms = document.querySelector('#sortable').querySelectorAll("form");
    for (i = 0; i < forms.length; i++) {
        console.log(forms[i]);
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
    var sortable = $('#sortable');
    sortable.sortable({
        update: function (event, ui) { sortableUpdated(event, ui); }
    });

});


function createQuestionForm(title = "", description = "") {
    var index = $("#sortable li").length;
    console.log("index: " + index);

    // Clone the template
    var template = document.getElementById("questionTemplate");
    var clone = document.importNode(template.content, true);

    var li = clone.querySelector("li");
    li.id = `qli${index}`;
    li.id = `qli${index}`;
    li.setAttribute("data", index);

    // Populate fields
    var form = clone.querySelector("form");
    form.id = `form${index}`;
    form.setAttribute("data", initialQuestions[index].Id);

    var select = form.querySelector("select");
    select.id = "selectQType" + index;
    select.name = `selectQType[${index}]`;
    select.value = initialQuestions[index].Type;
    select.setAttribute('data', select.value);
    console.log("select value: " + select.value);
    $(select).find("option:first").prop("disabled", true);

    $(select).selectmenu();
    $(select).selectmenu({
        open: function (event, ui) { onQTypeDropdownOpen(event, ui) },
        select: function (event, ui) { onQTypeDropdownChosen(event, ui) },
        close: function (event) { resetSelectedDDVal(event.target.value, event.target); }
    });

    $(select).next('.ui-selectmenu-button').on('mousedown', function (e) {
        console.log('mousedown on selectmenu button');
        e.stopImmediatePropagation(); // Prevents dragging behavior from triggering
    });

    var inputs = form.querySelectorAll("input");
    inputs[0].id = `title${index}`;
    inputs[0].name = `QuestionList[${index}].Title`;
    inputs[0].value = title;

    var textAreas = clone.querySelectorAll("textarea");
    textAreas[0].id = `description${index}`;
    textAreas[0].name = `QuestionList[${index}].Description`;
    textAreas[0].value = description;

    var spans = form.querySelectorAll(".templateValidation");
    spans[0].setAttribute("data-valmsg-for", `selectQType[${index}]`);
    spans[1].setAttribute("data-valmsg-for", `QuestionList[${index}].Title`);
    spans[2].setAttribute("data-valmsg-for", `QuestionList[${index}].Description`);

    var buttons = form.querySelectorAll("button");
    buttons.forEach(x => x.setAttribute("data", index));

    return clone;
}

var prevOpenDropdown = null;
function onQTypeDropdownOpen(event, ui) {
    console.log("open");
    var $select = $(event.target);

    // Close the currently open dropdown if it's not the same as this one
    if (prevOpenDropdown && prevOpenDropdown !== $select[0]) {
        console.log('close because');
        $(prevOpenDropdown).selectmenu("close");
    }
    prevOpenDropdown = $select[0]

    $select.children("option").each(function (index, option) {
        if (index == 0) return;
        var qType = questionTypes[index - 1];
        option.text = `${qType.DisplayName} (${qType.Left})`;
        $(option).prop("disabled", qType.Left <= 0);

    });
    $select.selectmenu("refresh");
}

function onQTypeDropdownChosen(event, ui) {
    var id = $(ui)[0].item.value;
    if (!id || isNaN(id)) return;

    var freedId = event.target.getAttribute("data");
    event.target.setAttribute("data", id);
    countQuestionTypeData(freedId, id);
}

function resetSelectedDDVal(id, select) {
    console.log('reset select dd val');
    console.log("id: " + id + " select: " + select);
    var qType = questionTypes.find(obj => {
        return obj.Id == id;
    });
    if (qType && id != '') {
        console.log("id found");
        $(select).find(`option[value="${id}"]`).text(qType.DisplayName); // Change the display name
    }
    $(select).selectmenu("refresh");
}

function countQuestionTypeData(freedId, chosenId) {
    console.log("on chosen: " + freedId + " cid: " + chosenId);
    if (freedId === chosenId)
        return;
    if (!isNaN(freedId) && freedId != '') {
        var questionType = questionTypes.find(x => x.Id == freedId);
        questionType.Left += 1;
    }
    questionType = questionTypes.find(x => x.Id == chosenId);
    questionType.Left -= 1;
}

function onSaveQuestion(args) {
    var id = args.getAttribute('data');
    var form = $(`#form${id}`);
    var data = form.serializeArray();

    if (!form.valid())
        return;

    var questionData = initialQuestions.find(x => x.Id == form.attr("data"));
    var questionClone = { ...questionData };
    questionClone.Type = data[0].value;
    questionClone.Title = data[1].value;
    questionClone.Description = data[2].value;


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
    var id = args.getAttribute('data');
    var form = $(`#form${id}`);
    var questionData = initialQuestions.find(x => x.Id == form.attr("data"));

    var li = form.parent("li");

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
    var questionButton = document.getElementById("CreateQButton");
    var isHidden = initialQuestions.length >= maxQuestionCount;
    if (isHidden) {
        questionButton.setAttribute('hidden', true);
    }
    else {
        questionButton.removeAttribute('hidden');
    }
}

function sortableUpdated(event, ui) {

    var sortable = $("#sortable");
    var idsInOrder = sortable.sortable("toArray");
    console.log(event.target.id);

    var reorderInfoArr = [];
    idsInOrder.forEach((value, index) => {
        var qId = sortable.find(`#${value}`).find("form").attr("data");
        reorderInfoArr.push({ "QId": qId, orderIndex: index });
    });

    //console.log('change order link: ' + changeOrderLink);
    
    var link = changeOrderLink;
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




