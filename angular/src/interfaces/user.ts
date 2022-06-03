import { IAnswer } from "./answer";

export interface IUser {
    userId: number,
    role: string,
    team: string,
    username: string,
    firstName: string,
    lastName: string,
    isActive: boolean,
    answers?:IAnswer[],
    pointsEarned?:number,
    pointsPossible?:number,
}