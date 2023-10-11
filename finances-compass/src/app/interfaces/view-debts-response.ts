import { Debt } from "./debt";

export interface ViewDebtsResponse {
    message: string | null;
    payload: Debt[];
    statusCode: number;
}
