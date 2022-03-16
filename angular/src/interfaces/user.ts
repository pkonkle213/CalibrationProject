import { IAnswer } from "./answer";

export interface IUser {
    userId: number,
    role: string,
    team: string,
    firstName: string,
    lastName: string,
    answers:IAnswer[],
}