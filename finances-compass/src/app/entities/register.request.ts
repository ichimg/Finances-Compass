export interface RegisterRequest {
    firstName: string;
    lastName: string;
    country: string;
    state: string;
    city: string;
    postalCode: string;
    streetAddress: string;
    username: string;
    email: string;
    phoneNumber: string;
    password: string;
    confirmPassword: string;
    currencyPreference: string;
    isDataConsent: boolean;
    clientURI: string;
}
