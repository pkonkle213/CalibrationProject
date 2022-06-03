export interface ISendUser {
    username:string,
    password:string,
    confirmPassword?:string,
    role?: string,
    team?: string,
    firstName?: string,
    lastName?: string,
    isActive: boolean,
}