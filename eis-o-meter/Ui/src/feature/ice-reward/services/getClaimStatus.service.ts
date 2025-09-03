import {ClaimIceRewardProps} from "@/feature/ice-reward/ClaimIceRewardProps.tsx";
import {GetClaimStatusType, GetClaimStatusResponse} from "@/feature/ice-reward/services/Contracts.ts";
import {ServiceUrls} from "@/feature/ice-reward/services/ServiceUrls.ts";

export const getClaimStatusService = async (
    email: string | null
): Promise<ClaimIceRewardProps> => {

    const url = new URL(ServiceUrls.getClaimStatus);
    
    if (email) {
        console.log("email", email);
        url.searchParams.append("email", email);
    }

    const res = await fetch(url.toString(), {
        method: "GET",
        headers: { "Content-Type": "application/json" },
    });

    if (!res.ok) {
        throw new Error(`Failed to fetch claim status: ${res.status}`);
    }

    const data: GetClaimStatusResponse = await res.json();
    console.log("API claim status response:", data);
    const props: ClaimIceRewardProps = {
        canClaim: data.status === GetClaimStatusType.ReadyToClaim,
        temperature: data.highestTemperature,
        message: buildClaimMessage(data.status, data.highestTemperature),
    };
    return props
};

const buildClaimMessage = (status: GetClaimStatusType, temperature: number): string => {
    switch (status) {
        case GetClaimStatusType.ReadyToClaim:
            return `🎉 It's ${temperature}°C! Hot enough for free ice cream!`;
        case GetClaimStatusType.AlreadyClaimed:
            return `✅ You've already claimed your ice cream. Enjoy!`;
        case GetClaimStatusType.Locked:
        default:
            return `No email, no ice cream 😅 Add your email and once it’s 30°C+, the ice cream is yours! Currently: ${temperature}°C.`;    }
};