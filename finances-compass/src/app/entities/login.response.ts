export interface LoginResponse {
  message: string | null;
  payload: {
    email: string;
    firstName: string;
    accessToken: string;
    refreshToken: string;
    currencyPreference: string;
    isDataConsent: boolean;
  };
  statusCode: number;
}
