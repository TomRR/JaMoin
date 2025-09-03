import { ClaimIceRewardProps } from "@/feature/ice-reward/ClaimIceRewardProps";
import { getClaimStatusService } from "@/feature/ice-reward/services/getClaimStatus.service";
import { GetClaimStatusType } from "@/feature/ice-reward/services/Contracts";

jest.mock("./ServiceUrls", () => ({
    ServiceUrls: {
        getClaimStatus: "http://test/api/claim-status",
        claimReward: "http://test/api/claim-reward",
    },
}));

describe("getClaimStatusService", () => {
    beforeEach(() => {
        global.fetch = jest.fn();
    });

    it("should return ReadyToClaim response correctly", async () => {
        const mockResponse = {
            date: "2025-09-02",
            highestTemperature: 35,
            lastUpdatedUtc: "2025-09-02T10:00:00Z",
            status: GetClaimStatusType.ReadyToClaim,
            code: "ICE123",
        };

        (global.fetch as jest.Mock).mockResolvedValue({
            ok: true,
            json: async () => mockResponse,
        });

        const result: ClaimIceRewardProps = await getClaimStatusService("test@example.com");

        expect(result).toEqual({
            canClaim: true,
            temperature: 35,
            code: "ICE123",
            message: `🎉 It's 35°C! Hot enough for free ice cream!`,
        });

        expect(global.fetch).toHaveBeenCalledWith(
            "http://test/api/claim-status?email=test%40example.com",
            expect.any(Object)
        );
    });

    it("should handle Claimed response correctly", async () => {
        const mockResponse = {
            date: "2025-09-02",
            highestTemperature: 36,
            lastUpdatedUtc: "2025-09-02T10:00:00Z",
            status: GetClaimStatusType.AlreadyClaimed,
            code: "ICE456",
        };

        (global.fetch as jest.Mock).mockResolvedValue({
            ok: true,
            json: async () => mockResponse,
        });

        const result = await getClaimStatusService(null);

        expect(result).toEqual({
            canClaim: false,
            temperature: 36,
            code: "ICE456",
            message: "✅ You've already claimed your ice cream. Enjoy!",
        });
    });

    it("should handle NotEligible response correctly", async () => {
        const mockResponse = {
            date: "2025-09-02",
            highestTemperature: 25,
            lastUpdatedUtc: "2025-09-02T10:00:00Z",
            status: GetClaimStatusType.Locked,
        };

        (global.fetch as jest.Mock).mockResolvedValue({
            ok: true,
            json: async () => mockResponse,
        });

        const result = await getClaimStatusService("foo@bar.com");

        expect(result).toEqual({
            canClaim: false,
            temperature: 25,
            code: undefined,
            message: "It's only 25°C. Need 30°C+ for free ice cream!",
        });
    });

    it("should throw error if fetch fails", async () => {
        (global.fetch as jest.Mock).mockResolvedValue({
            ok: false,
            status: 500,
        });

        await expect(getClaimStatusService("test@example.com")).rejects.toThrow(
            "Failed to fetch claim status: 500"
        );
    });
});
