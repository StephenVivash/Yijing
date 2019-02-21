import XCTest

#if !os(macOS)
public func allTests() -> [XCTestCaseEntry] {
    return [
        testCase(Swift2Tests.allTests),
    ]
}
#endif