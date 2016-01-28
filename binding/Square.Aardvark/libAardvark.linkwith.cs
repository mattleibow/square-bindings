using ObjCRuntime;

[assembly: LinkWith (
	"libAardvark-1.4.0.a", 
	LinkTarget.Simulator | LinkTarget.Simulator64 | LinkTarget.ArmV7 | LinkTarget.ArmV7s | LinkTarget.Arm64,
	SmartLink = true, 
	ForceLoad = true,
	Frameworks = "MessageUI QuartzCore",
	LinkerFlags = "-ObjC")]
