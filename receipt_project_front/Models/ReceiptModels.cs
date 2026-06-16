namespace receipt_project_front.Models;

// Mirrors backend records in App.Features.Receipt.ReceiptContracts.

public enum ReceiptStatus
{
    Pending,
    OcrProcessed,
    AiCategorized,
    Confirmed
}

public record ReceiptSummary(
    Guid ReceiptId,
    string StoreName,
    decimal? Amount,
    DateTimeOffset? PurchasedAt,
    string? Category,
    string? AiSuggestedCategory,
    ReceiptStatus Status,
    DateTimeOffset CreatedAt);

public record OcrResult(
    bool IsReceipt,
    string StoreName,
    decimal? Amount,
    DateTimeOffset? PurchasedAt,
    string RawText,
    IReadOnlyList<string> Warnings);

public record UploadReceiptResult(
    Guid ReceiptId,
    string ImagePath,
    OcrResult Ocr,
    string? AiSuggestedCategory,
    double? AiConfidence,
    Guid? AiLogId,
    ReceiptStatus Status,
    IReadOnlyList<string> Warnings);

public record ReceiptListResult(int Total, IReadOnlyList<ReceiptSummary> Items);

public record ReceiptDetail(
    Guid ReceiptId,
    string StoreName,
    decimal? Amount,
    DateTimeOffset? PurchasedAt,
    string? Category,
    string? AiSuggestedCategory,
    ReceiptStatus Status,
    string? RawOcrText,
    string? ContentType,
    DateTimeOffset CreatedAt,
    DateTimeOffset? ProcessedAt);

// 부분 수정(Partial Update): null로 보낸 필드는 변경되지 않는다.
public record UpdateReceiptRequest(
    string? StoreName = null,
    decimal? Amount = null,
    DateTimeOffset? PurchasedAt = null,
    string? Category = null);

public record UpdateReceiptResult(
    Guid ReceiptId,
    string StoreName,
    decimal? Amount,
    DateTimeOffset? PurchasedAt,
    string? Category,
    ReceiptStatus Status,
    DateTimeOffset UpdatedAt);

public record ConfirmReceiptCategoryRequest(string FinalCategory);

public record ConfirmReceiptCategoryResult(
    Guid ReceiptId,
    string FinalCategory,
    bool AiWasCorrect);

public record ApiError(string Message);

public record MonthlyTrend(int Year, int Month, int TotalCount, double TotalAmount);
