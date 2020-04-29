// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

let CurrentQuestion = 1;

const nextQuestion = function () {
    var QuestionCount = this.getAttribute("data-count")
    console.log("dit is de questioncount: " + QuestionCount)
    var questionCounter = document.querySelector("#questionCounter");
    var QuestionNumber = parseInt(questionCounter.innerHTML);
    QuestionNumber++;
    questionCounter.innerHTML = QuestionNumber
    if (QuestionCount != CurrentQuestion) {
        var questionItems = document.querySelectorAll('#question_' + (CurrentQuestion - 1));
        for (let item of questionItems) {
            item.classList.add('hide');
        }
        var newQuestionItems = document.querySelectorAll('#question_' + (CurrentQuestion));
        for (let item of newQuestionItems) {
            item.classList.remove('hide');
        }
        CurrentQuestion++;
        console.log("dit is de current: " + CurrentQuestion);
        if (CurrentQuestion == QuestionCount) {
            document.querySelector('#btnnext').classList.add("hide");
            document.querySelector('#btnsubmit').classList.remove("hide");
        }

    }
}
const imageChosen = function () {
    if (this.files.length == 0) {
        console.log("no file selected");
        var inputId = document.getElementById(this.getAttribute("data-guid"));
        console.log("dit is degene");
        console.log(inputId);
        inputId.value = 0;
    }
    else {
        console.log("file selected");
        console.log("no file selected");
        var inputId = document.getElementById(this.getAttribute("data-guid"));
        console.log("dit is degene");
        console.log(inputId);
        inputId.value = 1;
    }
}

const init = function () {
    try {
        var next = document.querySelector('#btnnext');
        next.addEventListener("click", nextQuestion);
    } catch (e) {

    }
    try {
        var images = document.querySelectorAll('.questionImage');
        for (let image of images) {
            image.addEventListener('change', imageChosen);
        };
    } catch (e) {

    }
    
}

document.addEventListener("DOMContentLoaded", init)