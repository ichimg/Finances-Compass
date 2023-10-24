export interface LoginResponse{
    message: string | null;
    payload: {
      email: string;
      accessToken: string;
      refreshToken: string;
    };
    statusCode: number;
}