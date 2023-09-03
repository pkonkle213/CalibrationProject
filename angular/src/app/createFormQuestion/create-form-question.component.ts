import { Component } from '@angular/core'
import { IForm } from 'src/interfaces/form';
import { IOption } from 'src/interfaces/option';
import { IQuestion } from 'src/interfaces/question';
import { FormService } from 'src/services/form.service';
import { QuestionService } from 'src/services/question.service';

@Component({
    selector: 'create-form-question',
    templateUrl: 'create-form-question.component.html',
    styleUrls: ['create-form-question.component.css'],
})

export class CreateFormComponent {
    forms:any;
    questions:any = [];
    newQuestion:boolean = false;
    newForm: boolean = false;
    questionIssue:boolean = false;
    updateMessage:string = "";
    updateSuccess:boolean = false;
    editFormName:boolean = false;
    editQuestion:number = 0;
    allOptions:any;

    selectedOptions:any[] = [];

    form:IForm = {
        formId: 0,
        formName: "",
        isArchived: false,
    }

    editOptionsForQuestion:IOption[] = [];

    createQuestion:IQuestion = {
        id: 0,
        formId: this.form.formId,
        questionText: "",
        pointsPossible: 0,
        formPosition: 0,
        options: [],
        isCategory: false,
    }

    constructor(private formService:FormService, private questionService:QuestionService) {
        
    }

    ngOnInit() {
        this.formService.getAllForms().subscribe((data) => {
            this.forms = data;
        });

        this.questionService.getAllOptions().subscribe((data) => {
            this.allOptions = data;
        });
    }

    SetOptions(question:IQuestion) {
        let availableOptions = this.OptionsForQuestion(question);
        let selectedOptions = question.options;

        availableOptions.array.forEach((aOption:IOption) => {
            if(!!selectedOptions.find((sOption:IOption) => aOption.id === sOption.id)) {

            }
            else {
               
            }
        });
    }

    OptionsForQuestion(question:IQuestion) {
        let availableOptions = this.allOptions.filter((option:IOption) => option.isCategory === question.isCategory);
        return availableOptions;
    }

    EditQuestion(question:IQuestion) {
        return (question.id === this.editQuestion);
    }

    SaveQuestion(question:IQuestion) {
        if (question.pointsPossible > 0) {
            question.isCategory = true;
        }
        else {
            question.isCategory = false;
        }

        for(let i = 0; i < this.questions.length; i++) {
            if (this.questions[i].id === question.id)
                this.questions[i] = question;
        }

        let myQuestion =
        [
            question,
        ]

        this.questionService.updateQuestions(myQuestion).subscribe((data) => {
            if(!data) {
                console.log("Didn't update question");
            }
            else {
                this.questions = this.questions;
                this.Cancel();
            }
        });
    }

    Disable(question:IQuestion) {
        console.log("Disabling question - " + question.id);
        console.log(question);
        // Eventually this will disable the question
    }

    SetEditQuestion(question:IQuestion) {
        this.editQuestion = question.id;
        this.createQuestion = question;
    }

    TotalPoints() {
        let sum:number = 0;
        for(let i = 0; i < this.questions.length; i++){
            sum += this.questions[i].pointsPossible;
        }

        return sum;
    }

    minPosition(question:IQuestion) {
        var minPosition = Math.min(...this.questions.map((x:IQuestion) => x.formPosition));
        return (question.formPosition == minPosition);
    }

    maxPosition(question:IQuestion) {
        var maxPosition = Math.max(...this.questions.map((x:IQuestion) => x.formPosition));
        return (question.formPosition == maxPosition);
    }
    
    CannotSubmitQuestion() {
        if (this.createQuestion.questionText == "")
            return true;

        if (isNaN(this.createQuestion.pointsPossible))
            return true;

        return false;
    }

    NewFormInput() {
        this.form.formId = 0;
        this.newForm = true;
        this.UpdateQuestionsTable();
    }

    SwitchIsArchived() {
        this.formService.switchIsArchived(this.form.formId).subscribe((data) => {
            if (!data) {
                console.log("well, that didn't work");
            }
            else {
                this.form.isArchived = !this.form.isArchived;
                console.log("Well something worked!");
            }
        });
    }

    Submit() {
        if (this.questions.length == 0) {
            this.createQuestion.formPosition = 1;
        }
        else {
            this.createQuestion.formPosition = Math.max(...this.questions.map((x:IQuestion) => x.formPosition)) + 1;
        }

        if (this.createQuestion.pointsPossible == null) {
            this.createQuestion.pointsPossible = 0;
        }

        this.createQuestion.formId = this.form.formId;

        this.questionService.submitQuestion(this.createQuestion).subscribe((data) => {
            if (!data) {
                this.questionIssue = true;
            }
            else {
                this.questions.push(data);
                this.createQuestion = {
                    id: 0,
                    formId: this.form.formId,
                    questionText: "",
                    pointsPossible: 0,
                    formPosition: 0,
                    options: [],
                    isCategory: false,
                }

                this.questionIssue = false;
                this.newQuestion = false;
            }
        });
    }

    EditFormName() {
        this.editFormName = true;
    }

    SaveFormName() {
        if (this.form.formId === 0) {
            this.formService.createForm(this.form).subscribe((data) => {
                if (!data) {
                    console.log("Couldn't create the form");
                }
                else {
                    this.formService.getAllForms().subscribe((forms) => {
                        this.forms = forms;
                    });   

                    this.editFormName = false;
                }
            });
        }
        else {
            this.formService.updateFormName(this.form).subscribe((data)=> {
                if (!data) {
                    console.log("Couldn't update the form");
                }
                else {
                    this.formService.getAllForms().subscribe((data) => {
                        this.forms = data;
                    });

                    this.editFormName = false;
                }
            });
        }
    }

    AddNewQuestion()
    {
        this.newQuestion = true;
    }

    ActiveForm(){
        return (this.form.formId > 0);
    }

    Cancel()
    {
        this.createQuestion = {
            id: 0,
            formId: this.form.formId,
            questionText: "",
            pointsPossible: 0,
            formPosition: 0,
            options: [],
            isCategory: false,
        }
        
        this.editQuestion = 0;
        this.newQuestion = false;
    }

    ShowQuestionsTable()
    {
        return (this.form.formId > 0 || this.newForm);
    }

    UpdateQuestionsTable() {
        var currentForm = this.forms.find((form:IForm) => form.formId == this.form.formId);
        if (currentForm != null) {
            this.form.formName = currentForm.formName;
            this.form.isArchived = currentForm.isArchived;
        }
        else {
            this.form.formName = "New Form Name";
        }

        this.questionService.getQuestionsByFormId(this.form.formId).subscribe((data) => {
            this.questions = data;
        });
    }

    SaveAndUpdateQuestions() {
        this.questionService.updateQuestions(this.questions).subscribe((data) => {
            if (!data) {
                this.updateMessage = "Something went wrong.";
            }
            else {
                this.updateMessage = "Successfully saved!";
                this.updateSuccess = true;
            }
        });

        console.log(this.updateMessage);
    }

    MoveDown(question:IQuestion) {
        var maxPosition = Math.max(...this.questions.map((x:IQuestion) => x.formPosition));
        if (question.formPosition == maxPosition)
            return;

        this.questions[question.formPosition].formPosition -= 1;
        this.questions[question.formPosition - 1].formPosition += 1;

        this.questions = this.questions.sort((a:IQuestion,b:IQuestion) => a.formPosition - b.formPosition);
    }

    MoveUp(question:IQuestion) {
        var minPosition = Math.min(...this.questions.map((x:IQuestion) => x.formPosition));
        if (question.formPosition == minPosition)
            return;

        this.questions[question.formPosition - 2].formPosition += 1;
        this.questions[question.formPosition - 1].formPosition -= 1;
        
        this.questions = this.questions.sort((a:IQuestion,b:IQuestion) => a.formPosition - b.formPosition);
    }
}