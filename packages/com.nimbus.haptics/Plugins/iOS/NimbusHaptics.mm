#import <Foundation/Foundation.h>
#import <UIKit/UIKit.h>

extern "C" {
    void Nimbus_PlayHaptic(int pattern) {
        if (@available(iOS 10.0, *)) {
            switch (pattern) {
                case 0: { // Light
                    UIImpactFeedbackGenerator *generator = [[UIImpactFeedbackGenerator alloc] initWithStyle:UIImpactFeedbackStyleLight];
                    [generator impactOccurred];
                    break;
                }
                case 1: { // Medium
                    UIImpactFeedbackGenerator *generator = [[UIImpactFeedbackGenerator alloc] initWithStyle:UIImpactFeedbackStyleMedium];
                    [generator impactOccurred];
                    break;
                }
                case 2: { // Heavy
                    UIImpactFeedbackGenerator *generator = [[UIImpactFeedbackGenerator alloc] initWithStyle:UIImpactFeedbackStyleHeavy];
                    [generator impactOccurred];
                    break;
                }
                case 3: { // Success
                    UINotificationFeedbackGenerator *generator = [[UINotificationFeedbackGenerator alloc] init];
                    [generator notificationOccurred:UINotificationFeedbackTypeSuccess];
                    break;
                }
                case 4: { // Warning
                    UINotificationFeedbackGenerator *generator = [[UINotificationFeedbackGenerator alloc] init];
                    [generator notificationOccurred:UINotificationFeedbackTypeWarning];
                    break;
                }
                case 5: { // Failure
                    UINotificationFeedbackGenerator *generator = [[UINotificationFeedbackGenerator alloc] init];
                    [generator notificationOccurred:UINotificationFeedbackTypeError];
                    break;
                }
                case 6: { // Selection
                    UISelectionFeedbackGenerator *generator = [[UISelectionFeedbackGenerator alloc] init];
                    [generator selectionChanged];
                    break;
                }
                default:
                    break;
            }
        }
    }
}
