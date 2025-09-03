import React from 'react';
import { Activity, DollarSign, TrendingUp, TrendingDown, CreditCard } from "lucide-react";

interface IconProps {
    iconType: 'dollar-sign' | 'activity-sign' | 'trending-up' | 'trending-down' | 'credit-card'; // Note the naming convention
    className?: string;
}

const Icon: React.FC<IconProps> = ({ iconType, className }) => {
    switch (iconType) {
        case 'dollar-sign':
            return <DollarSign className={className} />;
        case 'activity-sign':
            return <Activity className={className} />;
        case 'trending-up':
            return <TrendingUp className={className} />;
        case 'trending-down':
            return <TrendingDown className={className} />;
        case 'credit-card':
            return <CreditCard className={className} />;
        default:
            return null;
    }
};

export default Icon;
