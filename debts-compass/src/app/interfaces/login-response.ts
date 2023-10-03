export interface LoginResponse{
    message: string | null;
    payload: {
      email: string;
      token: string;
    };
    statusCode: number;
}