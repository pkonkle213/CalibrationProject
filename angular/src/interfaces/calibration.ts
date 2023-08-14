export interface ICalibration {
    id: number,
    repFirstName: string,
    repLastName: string,
    leaderUserId: number,
    groupScoreEarned: number,
    groupScorePossible: number,
    contactChannelId: number,
    calibrationDate: Date,
    contactId: string,
    isOpen: boolean,
    formId:number,
}