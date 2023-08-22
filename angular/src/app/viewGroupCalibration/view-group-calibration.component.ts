import { Component } from '@angular/core';
import { Router } from '@angular/router';
import { ActivatedRoute } from "@angular/router";
import { CalibrationService } from 'src/services/calibration.service';
import { IAnswer } from 'src/interfaces/answer';
import { IScore } from 'src/interfaces/score';
import { AuthService } from 'src/services/auth.service';
import { IQuestion } from 'src/interfaces/question';
import { ICalibration } from 'src/interfaces/calibration';
import { FormService } from 'src/services/form.service';

@Component({
    selector: 'view-group-calibration',
    templateUrl: 'view-group-calibration.component.html',
    styleUrls: ['view-group-calibration.component.css'],
})

export class GroupCalibrationComponent {
    calibration: any;
    calibrationId: number;
    contactTypes: any;
    contactType: string = "";
    questions: any;
    answers: any;
    groupSubmit: IAnswer[] = [];
    updating: boolean = false;
    participants: any;
    score: IScore = {
        calibrationId: 0,
        pointsEarned: 0,
        pointsPossible: 0,
    }

    constructor(private _calibrationService:CalibrationService, private _route:ActivatedRoute,private _router:Router, private auth:AuthService) {
        this.calibrationId = this._route.snapshot.params['id'];
    }

    ngOnInit() {
        this._calibrationService.getCalibration(this.calibrationId).subscribe(data => {
            this.calibration = data;
        });
       
        this._calibrationService.getAllContactTypes().subscribe((data) => {
            this.contactTypes = data;
        });

        this._calibrationService.getParticipants(this.calibrationId).subscribe(data => {
            this.participants = data;
            console.log("Participants:");
            console.log(this.participants);
        });
        
        this._calibrationService.getQuestions(this.calibrationId).subscribe(data => {
            this.questions = data;
            console.log("Questions:");
            console.log(this.questions);
        });

        this._calibrationService.getGroupAnswers(this.calibrationId).subscribe(data => {
            this.answers = data;
            console.log("Answers:");
            console.log(this.answers);

            if (this.answers===null || this.answers.length===0) {
                for(let i = 0; i < this.questions.length; i++) {
                    this.groupSubmit.push({
                        calibrationId: this.calibrationId,
                        questionId: this.questions[i].id,
                        optionValue: this.questions[i].options[0].optionValue,
                        comment: '',
                        pointsEarned: this.questions[i].options[0].pointsEarned,
                    })
                }
            }
            else {
                this.groupSubmit = this.answers;
                this.updating = true;
            }
        });
    }

    CanSubmitGroupScore() {
        return (this.auth.currentUser.user.role==="Admin" || (this.auth.currentUser.user.role==="Leader" && !this.updating));
    }

    AdminCheck() {
        return this.auth.currentUser.user.role==="Admin";
    }

    LeaderCheck() {
        return (this.auth.currentUser.user.role==="Leader" || this.auth.currentUser.user.role==="Admin");
    }

    Same(index:number) { //Should this be a foreach loop for simplicity? Or maybe a different linq method?
        let count = 0;
        for(let i = 0; i < this.participants.length; i++) {
            if(this.participants[i].answers[index].optionValue===this.groupSubmit[index].optionValue) {
                count++;
            }
        }

        return count;
    }

    Cancel() {
        this._router.navigate(['/view']);
    }

    Complete() {
        let total = 0;
        for(let i=0;i<this.questions.length;i++) {
            if(this.Different(i)===0) {
                total++;
            }
        }

        return total / this.questions.length * 100;
    }

    Different(index:number) {
        let count = 0;
        for(let i=0;i<this.participants.length;i++) {
            if(this.participants[i].answers[index].optionValue!==this.groupSubmit[index].optionValue) {
                count++;
            }
        }
        return count;
    }

    SumSame() {
        let sum = 0;
        for(let i=0;i < this.questions.length;i++) {
            sum += this.Same(i);
        }

        return sum;
    }

    SumDifferent() {
        let sum = 0;
        for(let i=0;i < this.questions.length; i++) {
            sum += this.Different(i);
        }

        return sum;
    }

    GetContactType() {
        var contact = this.contactTypes.find((type:any) => type.id === this.calibration.contactChannelId);
        return contact.name;
    }

    Calibrated(person:any) {
        let sum = 0;
        for(let i=0;i<this.questions.length;i++) {
            if(person.answers[i].optionValue===this.groupSubmit[i].optionValue) {
                sum++;
            }
        }

        return sum / this.questions.length * 100;
    }

    SwitchIsOpen() {
        this._calibrationService.switchIsOpen(this.calibrationId).subscribe((data) => {
            if(!data) {
                console.log("Could not switch");
            }
            else {
                this.calibration.isOpen = !this.calibration.isOpen;
            }
        });
    }

    Matching(questionId:any,answerValue:string) {
        let count = 0;
        for(let i=0;i<this.participants.length;i++) {
            if(this.participants[i].answers[questionId].optionValue===answerValue) {
                count++;
            }
        }

        return count;
    }

    Earned() {
        let earned = 0;
        
        for(let i=0;i<this.questions.length;i++) {
            earned += this.groupSubmit[i].pointsEarned * this.questions[i].pointsPossible;
        }

        return earned;
    }

    Possible() {
        let possible = 0;

        this.questions.map((e:IQuestion) => possible += e.pointsPossible);

        // for(let i=0;i<this.questions.length;i++) {
        //     possible += this.questions[i].pointsPossible
        // }

        return possible;
    }

    CalculateScore(earned:number,possible:number) {
        return earned / possible * 100;
    }

    BuildScore():IScore {
        this.score.calibrationId = this._route.snapshot.params['id'];
        this.score.pointsEarned = this.Earned();
        this.score.pointsPossible = this.Possible();
        return this.score;
    }
    
    SubmitGroupAnswer() {
        if(this.updating) {
            this._calibrationService.updateGroupAnswer(this.groupSubmit).subscribe(() => {});
            this._calibrationService.updateGroupScore(this.BuildScore()).subscribe(() => {
                this._router.navigate(['/view']);
            });
        }
        else {
            this._calibrationService.submitGroupAnswer(this.groupSubmit).subscribe(() => {});
            this._calibrationService.updateGroupScore(this.BuildScore()).subscribe(() => {
                this._router.navigate(['/view']);
            });
        }
    }

    onChange(event:any,i:number) {
        let q = this.questions[i].options.find((x: { optionValue: any; }) => x.optionValue===event.target.value)
        this.groupSubmit[i].pointsEarned = q.pointsEarned;
    }
}