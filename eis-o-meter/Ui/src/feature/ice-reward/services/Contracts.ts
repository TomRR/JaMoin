export interface GetClaimStatusResponse {
    lastUpdate: string;
    highestTemperature: number;
    status: GetClaimStatusType;
}

export enum GetClaimStatusType {
    Locked = "Locked",
    AlreadyClaimed = "AlreadyClaimed",
    ReadyToClaim = "ReadyToClaim",
}

export interface ClaimRewardResponse {
    status: ClaimRewardType;
}

export enum ClaimRewardType {
    SuccessfulClaimed = "SuccessfulClaimed",
    Expired = "Expired",
}