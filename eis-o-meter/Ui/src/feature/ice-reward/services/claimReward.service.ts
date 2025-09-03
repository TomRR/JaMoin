import {ServiceUrls} from "@/feature/ice-reward/services/ServiceUrls.ts";

export const claimRewardService = async (email: string): Promise<{
    code: string;
    status: string;
}> => {

    const url = new URL(ServiceUrls.claimReward);

    const res = await fetch(url, {
        method: "POST",
        headers: { "Content-Type": "application/json" },
        body: JSON.stringify({ email }),
    });

    if (!res.ok) {
        if (res.status === 400) {
            throw new Error("Something went wrong. Please check if the email address is valid.");
        }
        throw new Error(`Failed to claim reward: ${res.status}`);
    }

    const data: {
        status: string;
        code: string;
    } = await res.json();

    return {
        status: data.status,
        code: data.code,
    };
};