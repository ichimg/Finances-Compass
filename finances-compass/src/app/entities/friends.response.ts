import { UserFriend } from "./user-friend.model";

export interface FriendsResponse {
    message: string | null;
    payload: UserFriend[];
    statusCode: number;
}
