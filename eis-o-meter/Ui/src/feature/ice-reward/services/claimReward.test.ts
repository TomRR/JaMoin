import { claimRewardService } from "@/feature/ice-reward/services/claimReward.service";

jest.mock("./ServiceUrls", () => ({
    ServiceUrls: {
        getClaimStatus: "http://test/api/claim-status",
        claimReward: "http://test/api/claim-reward",
    },
}));

describe("claimRewardService", () => {
    beforeEach(() => {
        // reset fetch before each test
        global.fetch = jest.fn();
    });

    it("should POST email and return claim data on success", async () => {
        const mockResponse = {
            status: "Claimed",
            code: "ICE123",
        };

        (global.fetch as jest.Mock).mockResolvedValue({
            ok: true,
            json: async () => mockResponse,
        });

        const result = await claimRewardService("test@example.com");

        const fetchOptions = (global.fetch as jest.Mock).mock.calls[0][1];
        expect(fetchOptions.method).toBe("POST");
        expect(fetchOptions.body).toBe(JSON.stringify({ email: "test@example.com" }));
        expect(fetchOptions.headers).toEqual({ "Content-Type": "application/json" });

        expect(result).toEqual({
            status: "Claimed",
            code: "ICE123",
        });
    });

    it("should throw specific error for 400 response", async () => {
        (global.fetch as jest.Mock).mockResolvedValue({
            ok: false,
            status: 400,
        });

        await expect(claimRewardService("unknown")).rejects.toThrow(
            "Something went wrong. Please check if the email address is valid."
        );
    });

    it("should throw generic error for non-400 error response", async () => {
        (global.fetch as jest.Mock).mockResolvedValue({
            ok: false,
            status: 500,
        });

        await expect(claimRewardService("test@example.com")).rejects.toThrow(
            "Failed to claim reward: 500"
        );
    });
});
