import { HttpEventType } from '@angular/common/http';
import { Component } from '@angular/core';
import { Router } from '@angular/router';
import { ICalibration } from 'src/interfaces/calibration';
import { IScore } from 'src/interfaces/score';
import { CalibrationService } from 'src/services/calibration.service';

@Component ({
    templateUrl: 'view-all-calibrations.component.html',
    styleUrls: ['view-all-calibrations.component.css'],
})

export class ViewAllCalibrations {
    calibrations:any;
    calibration:any;
    scores:any;
    
    constructor(private _calibrationService: CalibrationService, private router:Router){
    }
    
    ngOnInit() {
        this._calibrationService.getAllCalibrations().subscribe(data => {
            this.calibrations = data;
        });
        this._calibrationService.getMyScores().subscribe(data => {
            this.scores = data;
        })
    }

    ConvertToPercent(earned:number, possible: number) {
        return earned / possible * 100;
    }

    PushToOne(id:number) {
        this.router.navigate(['/view/',id]);
    }

    FindEarned(calibrationId:number) {
        let earned=0;
        
        for(let i=0;i<this.scores.length;i++) {
            if(this.scores[i].calibrationId===calibrationId) {
                earned=this.scores[i].pointsEarned;
            }
        }

        return earned;
    }

    FindPossible(calibrationId:number) {
        let possible=0;
        for(let i=0;i<this.scores.length;i++) {
            if(this.scores[i].calibrationId===calibrationId) {
                possible=this.scores[i].pointsPossible;
            }
        }

        return possible;
    }

    PushToGroup(id:number) {
        this.router.navigate(['/group/',id]);
    }
}