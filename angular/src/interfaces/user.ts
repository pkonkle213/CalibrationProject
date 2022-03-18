import { IAnswer } from "./answer";

export interface IUser {
    userId: number,
    role: string,
    team: string,
    username: string,
    firstName: string,
    lastName: string,
    answers?:IAnswer[],
}