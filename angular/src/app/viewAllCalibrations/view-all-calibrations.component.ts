import { Component } from '@angular/core';
import { Router } from '@angular/router';
import { ICalibration } from 'src/interfaces/calibration';
import { IContactType } from 'src/interfaces/contactType';
import { AuthService } from 'src/services/auth.service';
import { CalibrationService } from 'src/services/calibration.service';

@Component ({
    templateUrl: 'view-all-calibrations.component.html',
    styleUrls: ['view-all-calibrations.component.css'],
})

export class ViewAllCalibrations {
    allCalibrations:any;
    calibration:any;
    scores:any;
    types:any;
    isOpen:boolean = true;
    
    constructor(private _calibrationService: CalibrationService, private router:Router, private auth:AuthService){

    }
    
    ngOnInit() {
        this._calibrationService.getAllCalibrations().subscribe(data => {
            this.allCalibrations = data;
        });

        this._calibrationService.getMyScores().subscribe(data => {
            this.scores = data;
        });

        this._calibrationService.getAllContactTypes().subscribe((data) => {
            this.types = data;
        });
    }

    SwitchOpen() {
        this.isOpen = !this.isOpen
    }

    FilterCalibrations() {
        if (this.isOpen)
            return (this.allCalibrations.filter((calibration:ICalibration) => calibration.isOpen === true));
        
        return (this.allCalibrations);
    }

    Wait(calibration:ICalibration) {
        return (!this.auth.LeaderCheck(calibration.leaderUserId) && calibration.groupScorePossible === 0);
    }

    Start(calibration:ICalibration) {
        return (this.auth.LeaderCheck(calibration.leaderUserId) && calibration.groupScorePossible === 0);
    }

    GetContactChannelName(channelId:number) {
        return (this.types.find((x:IContactType) => x.id === channelId).name);
    }

    ViewEdit(calibration:ICalibration) {
        return (!!calibration.groupScorePossible);
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