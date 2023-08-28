export interface IUser {
    userId: number,
    role: string,
    teamId: number,
    username: string,
    firstName: string,
    lastName: string,
    isActive: boolean,
    calibrationPosition?: number,
}