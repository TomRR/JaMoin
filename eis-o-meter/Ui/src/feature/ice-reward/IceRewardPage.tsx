import { useState, useEffect } from "react";
import { Button } from "@/components/ui/button";
import {
    Card,
    CardContent,
    CardDescription,
    CardHeader,
    CardTitle,
} from "@/components/ui/card";
import { Gift, RefreshCw, Thermometer } from "lucide-react";
import { ClaimIceRewardProps } from "@/feature/ice-reward/ClaimIceRewardProps";
import { Input } from "@/components/ui/input";
import {getClaimStatusService} from "@/feature/ice-reward/services/getClaimStatus.service.ts";
import {claimRewardService} from "@/feature/ice-reward/services/claimReward.service.ts";
import { ToastContainer, toast } from "react-toastify";
import "react-toastify/dist/ReactToastify.css";

export default function IceRewardPage() {
    const [claimData, setClaimData] = useState<ClaimIceRewardProps | null>(null);
    const [isLoading, setIsLoading] = useState(false);
    const [hasClaimed, setHasClaimed] = useState(false);
    const [error, setError] = useState<string | null>(null);
    const [email, setEmail] = useState("");

    const checkTemperature = async () => {
        setIsLoading(true);
        setError(null);

        try {
            const data = await getClaimStatusService(email);
            setClaimData(data);
        } catch (err) {
            setError((err as Error).message);
        } finally {
            setIsLoading(false);
        }
    };

    const handleClaimReward = async () => {
        if (!email) {
            setError("Please enter your valid email before claiming.");
            return;
        }

        setIsLoading(true);
        setError(null);

        try {
            const result = await claimRewardService(email);
            setHasClaimed(true);
            setClaimData((prev) =>
                prev ? { ...prev, code: result.code, message: `Status: ${result.status}` } : prev
            );
        } catch (err) {
            setError((err as Error).message);
        } finally {
            setIsLoading(false);
        }
    };
    useEffect(() => {
        if (claimData?.canClaim) {
            toast.info("🌡️ It's hot enough! You can claim your ice cream now.");
        }
    }, [claimData]);
    useEffect(() => {
        const interval = setInterval(() => {
            checkTemperature();
        }, 600000); 

        return () => clearInterval(interval); // cleanup on unmount
    }, [email]); 

    return (
        <div className="min-h-screen bg-gradient-to-br from-blue-50 to-emerald-50 flex items-center justify-center p-4">
            <div className="w-full max-w-md space-y-6">
                <ToastContainer position="top-center" autoClose={3000} />
                {/* Header */}
                <div className="text-center space-y-2">
                    <div className="text-6xl mb-4">🍦</div>
                    <h1 className="text-3xl font-bold text-primary text-balance">
                        Summer Ice Cream Claim
                    </h1>
                    <p className="text-muted-foreground text-pretty">
                        When it hits 30°C, claim your free ice cream!
                    </p>
                </div>
                <Input
                    type="email"
                    placeholder="Enter your email"
                    value={email}
                    onChange={(e) => setEmail(e.target.value)}
                    disabled={isLoading}
                />
                {/* Main Card */}
                <Card className="shadow-lg border-0 bg-card/80 backdrop-blur-sm">
                    <CardHeader className="text-center">
                        <CardTitle className="flex items-center justify-center gap-2">
                            <Thermometer className="h-5 w-5 text-primary" />
                            Temperature Check
                        </CardTitle>
                        <CardDescription>
                            Click refresh to check if you can claim your ice cream
                        </CardDescription>
                    </CardHeader>

                    <CardContent className="space-y-4">
                        {/* Refresh Button */}
                        <Button
                            onClick={checkTemperature}
                            disabled={isLoading}
                            className="w-full bg-primary hover:bg-primary/90"
                            size="lg"
                        >
                            {isLoading ? (
                                <>
                                    <RefreshCw className="mr-2 h-4 w-4 animate-spin" />
                                    Checking Temperature...
                                </>
                            ) : (
                                <>
                                    <RefreshCw className="mr-2 h-4 w-4" />
                                    Check Temperature
                                </>
                            )}
                        </Button>

                        {/* Error */}
                        {error && (
                            <div className="p-3 bg-red-100 text-red-700 rounded text-sm text-center">
                                {error}
                            </div>
                        )}

                        {/* Results */}
                        {claimData && !error && (
                            <div className="space-y-4 animate-in fade-in-50 duration-500">
                                {/* Temperature Display */}
                                <div className="text-center p-4 rounded-lg bg-muted">
                                    <div className="text-2xl font-bold text-primary">
                                        {claimData.temperature}°C
                                    </div>
                                    <p className="text-sm text-muted-foreground mt-1">
                                        Current Temperature
                                    </p>
                                </div>

                                {/* Status Message */}
                                <div
                                    className={`p-4 rounded-lg text-center ${
                                        claimData.canClaim
                                            ? "bg-emerald-50 text-emerald-700 border border-emerald-200"
                                            : "bg-orange-50 text-orange-700 border border-orange-200"
                                    }`}
                                >
                                    <p className="font-medium">{claimData.message}</p>
                                </div>

                                {/* Claim Section */}
                                {claimData.canClaim && !hasClaimed && (
                                    <div className="space-y-3">
                                        <Button
                                            onClick={handleClaimReward}
                                            disabled={isLoading}
                                            className="w-full bg-accent hover:bg-accent/90 text-accent-foreground"
                                            size="lg"
                                        >
                                            <Gift className="mr-2 h-4 w-4" />
                                            Claim Free Ice Cream!
                                        </Button>
                                    </div>
                                )}

                                {/* Claimed Code */}
                                {hasClaimed && (
                                    <div className="p-4 bg-emerald-100 rounded-lg text-center border border-emerald-300">
                                        <p className="text-sm text-emerald-700 mb-2">Claimed</p>
                                    </div>
                                )}
                            </div>
                        )}
                    </CardContent>
                </Card>

                {/* Footer */}
                <div className="text-center text-sm text-muted-foreground">
                    <p>🌞 Stay cool and enjoy your summer treats! 🌞</p>
                </div>
            </div>
        </div>
    );
}
