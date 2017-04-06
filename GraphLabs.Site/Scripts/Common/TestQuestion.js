var addOption = (question) => {
    var id_max = $("#answers").children('tbody').children('tr').size();
    $("#answers").children('tbody').append('' +
        '<tr id="' + id_max + '">' +
        '<td>' +
        '<input type="hidden" value="0" id="id_' + id_max + '"/>' +
        '<input type="text" value="" id="answer_' + id_max + '"/>' +
        '<input type="hidden" value="' + question + '" id="question_' + id_max + '"/>' +
        '</td>' +
        '<td>' +
        '<input type="checkbox" value="value" id="isCorrect_' + id_max + '"/>' +
        '</td>' +
        '<td>' +
        '<img title="Клонировать ответ" src="/Images/copy.png" onclick="(' + id_max + ');" />' +
        '<img title="Удалить ответ" src="/Images/_false.png" onclick="addOption(' + id_max + ');" />' +
        '</td>' +
        '</tr>');
    $.ajax({
        url: '/Answer/Create',
        method: 'POST',
        data: JSON.stringify({
            Id: 0,
            Answer: $("#answer_" + id_max).val(),
            IsCorrect: false,
            TestQuestion: parseInt($("#questionId").text())
        }),
        contentType: "application/json; charset=utf-8",
        success: (data) => {
            console.log(data);
            if (data !== false) {
                $("#id_" + id_max).val(data);
                $("#systemMessage").html("<div class=\"alert alert-success\" role=\"alert\">" +
            "<button type=\"button\" class=\"close\" data-dismiss=\"alert\" aria-label=\"Close\"><span aria-hidden=\"true\">&times;</span></button>" +
            "Значение успешно изменилось!</div>");
            } else {
                deleteOption(id_max);
                $("#systemMessage").html("<div class=\"alert alert-danger\" role=\"alert\">" +
            "<button type=\"button\" class=\"close\" data-dismiss=\"alert\" aria-label=\"Close\"><span aria-hidden=\"true\">&times;</span></button>" +
            "Значение не изменилось, повторите позже!</div>");
            }
        }
    });
};

var sendModel = () => {
    var answers = [];
    var trs = $("#answers").children('tbody').children('tr');
    for (var i = 0; i < $(trs).length; i++) {
        var answer = {};
        answer.Id = parseInt($($($(trs).get(i)).children('td').get(0)).children('#id_' + i).val());
        answer.Answer = $($($(trs).get(i)).children('td').get(0)).children('#answer_' + i).val();
        answer.IsCorrect = $($($(trs).get(i)).children('td').get(1)).children('#isCorrect_' + i).prop('checked');
        answer.TestQuestion = parseInt($($($(trs).get(i)).children('td').get(0)).children('#question_' + i).val());
        answers.push(answer);
    }
    $.ajax({
        url: '/Survey/Edit',
        method: "POST",
        data: JSON.stringify({
            Id: parseInt($("#questionId").text()),
            Question: $("#question").val(),
            Category: {
                Id: parseInt($("#categoryId").text()),
                Name: $("#categoryName").text()
            },
            AnswerVariants: answers
        }),
        contentType: "application/json; charset=utf-8",
        success: (data) => {
            if (data) {
                $("#systemMessage").html("<div class=\"alert alert-success\" role=\"alert\">" +
            "<button type=\"button\" class=\"close\" data-dismiss=\"alert\" aria-label=\"Close\"><span aria-hidden=\"true\">&times;</span></button>" +
            "Значение успешно изменилось!</div>");
            } else {
                $("#systemMessage").html("<div class=\"alert alert-danger\" role=\"alert\">" +
            "<button type=\"button\" class=\"close\" data-dismiss=\"alert\" aria-label=\"Close\"><span aria-hidden=\"true\">&times;</span></button>" +
            "Значение не изменилось, повторите позже!</div>");
            }
        }
    });
};

var deleteOption = (id) => {
    $.ajax({
        url: '/Answer/Delete',
        method: 'POST',
        data: JSON.stringify({
            Id: parseInt($("#id_" + id).val()),
            Answer: $("#answer_" + id).val(),
            IsCorrect: $("#isCorrect_" + id).val(),
            TestQuestion: parseInt($("#questionId").text())
        }),
        contentType: 'application/json; charset=utf-8',
        success: (data) => {
            if (data) {
                var trs = $("#answers").children('tbody').children('tr');
                $("#" + id).remove();
                var trsNew = $("#answers").children('tbody').children('tr');
                var i = 0;
                for (var j = 0; j < $(trs).length; j++) {
                    if ($("#" + j).length) {
                        if (j !== i) {
                            $($(trsNew).get(i)).children(':nth-child(1)').children('#id_' + j).attr('id', i);
                            $($(trsNew).get(i)).children(':nth-child(1)').children('#answer_' + j).attr('id', i);
                            $($(trsNew).get(i)).children(':nth-child(1)').children('#question_' + j).attr('id', i);
                            $($(trsNew).get(i)).children(':nth-child(2)').children('#isCorrect_' + j).attr('id', i);
                            $($(trsNew).get(i)).attr('id', i);
                        }
                        i++;
                        if (i === $(trsNew.length)) break;
                    }
                }
                $("#systemMessage").html("<div class=\"alert alert-success\" role=\"alert\">" +
            "<button type=\"button\" class=\"close\" data-dismiss=\"alert\" aria-label=\"Close\"><span aria-hidden=\"true\">&times;</span></button>" +
            "Значение успешно изменилось!</div>");
            } else {
                $("#systemMessage").html("<div class=\"alert alert-danger\" role=\"alert\">" +
            "<button type=\"button\" class=\"close\" data-dismiss=\"alert\" aria-label=\"Close\"><span aria-hidden=\"true\">&times;</span></button>" +
            "Значение не изменилось, повторите позже!</div>");
            }
        }
    });
};

var copyOption = (id) => {
    $.ajax({
        url: '/Answer/Create',
        method: 'POST',
        data: JSON.stringify({
            Id: 0,
            Answer: $("#answer_" + id).val(),
            IsCorrect: $("#isCorrect_" + id).val(),
            TestQuestion: parseInt($("#questionId").text())
        }),
        contentType: 'application/json; charset=utf-8',
        success: (data) => {
            if (data !== false) {
                var trs = $("#answers").children('tbody').children('tr');
                var answer = $($($(trs).get(i)).children('td').get(0)).children('#answer_' + i).val();
                var isCorrect = $($($(trs).get(i)).children('td').get(1)).children('#isCorrect_' + i).prop('checked');
                var testQuestion = $($($(trs).get(i)).children('td').get(0)).children('#question_' + i).val();
                var id_max = $("#answers").children('tbody').children('tr').size();
                var checked = isCorrect ? 'checked' : '';
                $("#answers").children('tbody').append('' +
                    '<tr id="' + id_max + '">' +
                    '<td>' +
                    '<input type="hidden" value="' + data + '" id="id_' + id_max + '"/>' +
                    '<input type="text" value="' + answer + '" id="answer_' + id_max + '"/>' +
                    '<input type="hidden" value="' + testQuestion + '" id="question_' + id_max + '"/>' +
                    '</td>' +
                    '<td>' +
                    '<input type="checkbox" value="value" id="isCorrect_' + id_max + '" ' + checked + '/>' +
                    '</td>' +
                    '<td>' +
                    '<img title="Клонировать ответ" src="/Images/copy.png" onclick="copyOption(' + (id_max + 1) + ');" />' +
                    '<img title="Удалить ответ" src="/Images/_false.png" onclick="addOption(' + id_max + ');" />' +
                    '</td>' +
                    '</tr>');
            }
        }
    });
};