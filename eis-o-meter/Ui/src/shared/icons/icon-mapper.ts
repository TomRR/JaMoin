import React from 'react';
import {IconType} from "@/shared/icons/IconType.ts";

import {
    DollarSign,
    Briefcase, // Using Briefcase for Portfolio
    CreditCard,
    Users,
    Activity,
    HelpCircle,
    Percent,
    Euro, ArrowUp, ArrowDown,
} from 'lucide-react';

/**
 * A mapping from IconType enum members to their corresponding React icon components.
 * The keys are IconType enum values, and the values are the icon components.
 * Using React.ElementType allows for any valid React component type.
 */
export const iconMap: Record<IconType, React.ElementType> = {
    [IconType.Dollar]: DollarSign,
    [IconType.Portfolio]: Briefcase,
    [IconType.Transactions]: CreditCard,
    [IconType.Users]: Users,
    [IconType.Activity]: Activity,
    [IconType.CreditCard]: CreditCard, 
    [IconType.Percent]: Percent, 
    [IconType.Euro]: Euro, 
};

/**
 * Retrieves the icon component based on the IconType.
 * If the iconType is not found in the map, it returns a default icon.
 * @param iconType The type of icon to retrieve.
 * @returns The React component for the icon.
 */
export const getIconComponent = (iconType: IconType): React.ElementType => {
    return iconMap[iconType] || HelpCircle; // Fallback to HelpCircle if no icon is found
};

export const getArrowIconComponent = (isPositive: boolean): React.ElementType => {
    return isPositive ? ArrowUp : ArrowDown;
};
