import { Component } from '@angular/core';
import { Router } from '@angular/router';
import { ActivatedRoute } from "@angular/router";
import { CalibrationService } from 'src/services/calibration.service';
import { IAnswer } from 'src/interfaces/answer';

@Component({
    selector: 'view-group-calibration',
    templateUrl: 'view-group-calibration.component.html',
    styleUrls: ['view-group-calibration.component.css'],
})

export class GroupCalibrationComponent {
    calibration:any;
    calibrationId:number;
    questions:any;
    answers:any;
    groupSubmit:IAnswer[] = [];
    updatingAnswer:boolean = false;
    participants:any;

    constructor(private _calibrationService:CalibrationService, private _route:ActivatedRoute,private _router:Router) {
        this.calibrationId=this._route.snapshot.params['id'];
    }

    ngOnInit() {
        this._calibrationService.getCalibration(this.calibrationId).subscribe(data => {
            this.calibration = data;
        });

        this._calibrationService.getQuestions(this.calibrationId).subscribe(data => {
            this.questions = data;
        });

        this._calibrationService.getParticipants(this.calibrationId).subscribe(data => {
            this.participants = data;
            console.log(this.participants);
        });

        this._calibrationService.getGroupAnswers(this.calibrationId).subscribe(data => {
            this.answers = data;
        })
    }


    SubmitGroupAnswer() {

    }
}