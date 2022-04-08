import { Component } from '@angular/core';
import { Router } from '@angular/router';
import { ICalibration } from 'src/interfaces/calibration';
import { AuthService } from 'src/services/auth.service';
import { CalibrationService } from 'src/services/calibration.service';

@Component ({
    templateUrl: 'view-all-calibrations.component.html',
    styleUrls: ['view-all-calibrations.component.css'],
})

export class ViewAllCalibrations {
    calibrations:any;
    calibration:any;
    scores:any;
    
    constructor(private _calibrationService: CalibrationService, private router:Router, private auth:AuthService){
    }
    
    ngOnInit() {
        this._calibrationService.getAllCalibrations().subscribe(data => {
            this.calibrations = data;
        });
        this._calibrationService.getMyScores().subscribe(data => {
            this.scores = data;
        })
    }

    Wait(calibration:ICalibration) {
        if (!this.LeaderCheck() && calibration.groupScorePossible===0){
            return true;
        }
        
        return false;
    }

    Start(calibration:ICalibration) {
        if (this.LeaderCheck() && calibration.groupScorePossible===0) {
            return true;
        }

        return false;
    }

    ViewEdit(calibration:ICalibration) {
        if(!!calibration.groupScorePossible) {
            return true;
        }

        return false;
    }

    LeaderCheck() {
        if(this.auth.currentUser.user.role==="Leader" || this.auth.currentUser.user.role==="Admin"){
            return true;
        }

        return false;
    }

    AdminCheck() {
        if(this.auth.currentUser.user.role==="Admin"){
            return true;
        }

        return false;
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
        //this._calibrationService.switchIsOpen(id).subscribe(data => {});
        this.router.navigate(['/group/',id]);
    }
}