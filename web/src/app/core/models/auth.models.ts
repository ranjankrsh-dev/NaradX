export interface AuthResponse {
    userId: number;
    email: string;
    accessToken: string;
    accessTokenExpires: string;
    refreshToken: string;
    refreshTokenExpires: string;
    message: string;
}
