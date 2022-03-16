import { Component } from '@angular/core';
import { IAnswer } from 'src/interfaces/answer';
import { CalibrationService } from 'src/services/calibration.service';
import { Router } from '@angular/router';
import { ActivatedRoute } from "@angular/router";

@Component({
    selector: 'view-group-calibration',
    templateUrl: 'view-group-calibration.component.html',
    styleUrls: ['view-group-calibration.component.css'],
})

export class GroupCalibrationComponent {
    calibration:any;
    questions:any;
    answers:any;
    groupSubmit:IAnswer[] = [];
    updatingAnswer:boolean = false;

    constructor(private _calibrationService:CalibrationService, private _route:ActivatedRoute,private _router:Router) {

    }

    ngOnInit() {

    }


    SubmitGroupAnswer() {

    }

    CheckForDeduction(index:number) {

    }
}