export interface UserToLoginDTO {
    username: string;
    password: string;
}

export interface UserToRegisterDTO {
    username: string;
    password: string;
    name: string;
    surname: string;
}

export interface RefreshTokenDTO {
    token: string;
}
