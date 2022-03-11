import {Component, OnInit, Input, Output, EventEmitter} from '@angular/core';
import { IAnswer } from 'src/interfaces/answer';
import { ICalibration } from 'src/interfaces/calibration';
import { IDetail } from 'src/interfaces/detail';
import { IParticipant } from 'src/interfaces/participant';
import { IQuestion } from 'src/interfaces/question';
import { CalibrationService } from 'src/services/calibration.service';

@Component({
    selector: 'completed-calibration',
    templateUrl: 'calibration.component.html',
    styleUrls: ['calibration.component.css'],
})

export class CompletedCalibrationComponent {
    participants:IParticipant[] = [];
    // answers:IAnswer[] = this._calibrationService.getAnswers(1);
    questions:IQuestion[] = [];
    calibration:any; // I can't apply ICalibration to it due to it maybe being null
    // groupScore:IAnswer;

    groupSubmit:IDetail[] = [];

    constructor(private _calibrationService: CalibrationService) {
        // this.answers = this._calibrationService.getAnswers(1);
        // this.questions = this._calibrationService.getQuestions(1);
        // this.calibration = this._calibrationService.getCalibration(1);
        // this.groupScore = this._calibrationService.getGroupScore(1);

        for(let i=0;i<this.questions.length;i++)
        {
            this.groupSubmit.push({
                option:"Meets",
                comment:""
            })
        }
    }

    ngOnInit() {
        // this._calibrationService.getParticipants(1).subscribe(data => {
            // this.participants = data;
        // });
    }


    SubmitGroupAnswer() {
        // console.log(this.groupSubmit);
        // if (this.calibration!=null){
        //     this.calibration.isOpen=false;
        // }
        // this.groupScore={participantId:0,details:this.groupSubmit};
        // send the update to the database as well
    }

    CheckForDeduction(index:number) {
        if (this.groupSubmit[index].option==='Does Not Meet') {
            return true;
        }

        return false;
    }
}