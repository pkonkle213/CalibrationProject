export interface ICalibration {
    id: number;
    repFirstName: string;
    repLastName: string;
    groupScoreEarned: number;
    groupScorePossible: number;
    contactChannel: string;
    calibrationDate: Date;
    contactId: string;
    isActive: boolean;
    indivPointsEarned: number;
    indivPointsPossible: number;
}