using LoveQuiz.Server.Models;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;

public sealed class FinalReportDocumentPdf : IDocument
{
    private readonly FinalReport _r;
    public FinalReportDocumentPdf(FinalReport report) => _r = report;

    public DocumentMetadata GetMetadata() => new();

    public void Compose(IDocumentContainer container)
    {
        var bg = Colors.Grey.Lighten5;
        var primarySoft = Colors.Pink.Lighten4;
        var border = Colors.Grey.Lighten2;

        container.Page(page =>
        {
            page.Size(PageSizes.A4);
            page.Margin(28);
            page.DefaultTextStyle(t => t.FontSize(10).LineHeight(1.15f));

            page.Content().Background(bg).Padding(10).Column(col =>
            {
                // HEADLINE
                col.Item().Column(head =>
                {
                    head.Item().Container().AlignCenter().Text("RELATIA TA ESTE DE TIPUL:")
                        .FontSize(9).SemiBold().FontColor(Colors.Grey.Darken1);

                    head.Item().Container().AlignCenter().Text(_r.AttachmentStyle.Label?.ToUpperInvariant() ?? "")
                        .FontSize(22).Bold();

                    if (!string.IsNullOrWhiteSpace(_r.AttachmentStyle.Nickname))
                        head.Item().Container().AlignCenter().Text($"~ {_r.AttachmentStyle.Nickname} ~")
                            .FontSize(10).Italic().FontColor(Colors.Grey.Darken1);
                });

                // CE ÎNSEAMNĂ ASTA?
                col.Item().Border(1).BorderColor(border).Padding(8).Column(sec =>
                {
                    sec.Item().Container()
                        .Background(primarySoft).PaddingVertical(3).PaddingHorizontal(6)
                        .Text("Ce înseamnă asta?").SemiBold().FontSize(11);

                    sec.Item().PaddingTop(4).Container().Text(
                        string.IsNullOrWhiteSpace(_r.AttachmentStyle.Summary)
                            ? _r.AttachmentStyleShortText
                            : _r.AttachmentStyle.Summary
                    );
                });

                // WORKS / NOT WORKS
                col.Item().PaddingTop(6).Row(row =>
                {
                    row.RelativeItem().Border(1).BorderColor(border).Padding(8).Column(sec =>
                    {
                        sec.Item().Container()
                            .Background(primarySoft).PaddingVertical(3).PaddingHorizontal(6)
                            .Text("Ce funcționează?").SemiBold().FontSize(11);

                        sec.Item().PaddingTop(4).Column(list =>
                        {
                            foreach (var n in _r.MetNeeds.Take(4))
                                list.Item().Row(r =>
                                {
                                    r.ConstantItem(14).AlignMiddle().Container()
                                        .AlignCenter().Text("✔").FontSize(9).FontColor(Colors.Green.Medium);

                                    r.RelativeItem().Container().PaddingLeft(2)
                                        .Text(n.Title).LineHeight(1.15f);
                                });
                        });
                    });

                    row.Spacing(8);

                    row.RelativeItem().Border(1).BorderColor(border).Padding(8).Column(sec =>
                    {
                        sec.Item().Container()
                            .Background(primarySoft).PaddingVertical(3).PaddingHorizontal(6)
                            .Text("Ce NU funcționează?").SemiBold().FontSize(11);

                        sec.Item().PaddingTop(4).Column(list =>
                        {
                            foreach (var n in _r.UnmetNeeds.Take(4))
                                list.Item().Row(r =>
                                {
                                    r.ConstantItem(14).AlignMiddle().Container()
                                        .AlignCenter().Text("✖").FontSize(9).FontColor(Colors.Red.Medium);

                                    r.RelativeItem().Container().PaddingLeft(2)
                                        .Text(n.Title).LineHeight(1.15f);
                                });
                        });
                    });
                });

                // ASPECTE
                col.Item().PaddingTop(6).Border(1).BorderColor(border).Padding(8).Column(sec =>
                {
                    sec.Item().Container()
                        .Background(primarySoft).PaddingVertical(3).PaddingHorizontal(6)
                        .Text("Aspecte ale relației tale").SemiBold().FontSize(11);

                    sec.Item().PaddingTop(4).Column(list =>
                    {
                        foreach (var a in _r.Aspects.Take(3))
                            list.Item().PaddingBottom(2).Container().Text(t =>
                            {
                                t.Span($"{a.Aspect}: ").SemiBold();
                                t.Span(a.Description);
                            });
                    });
                });

                // TOXIC HABITS
                col.Item().PaddingTop(6).Border(1).BorderColor(border).Padding(8).Column(sec =>
                {
                    sec.Item().Container()
                        .Background(primarySoft).PaddingVertical(3).PaddingHorizontal(6)
                        .Text("Trăsătura ta nesănătoasă").SemiBold().FontSize(11);

                    if (!string.IsNullOrWhiteSpace(_r.ToxicHabitsSection.Title))
                        sec.Item().PaddingTop(4).Container().Text(_r.ToxicHabitsSection.Title).SemiBold();

                    sec.Item().PaddingTop(3).Column(list =>
                    {
                        foreach (var h in _r.ToxicHabitsSection.Habits.Take(3))
                            list.Item().PaddingBottom(2).Container().Text(t =>
                            {
                                t.Span("• ").SemiBold();
                                t.Span($"{h.Title}: ").SemiBold();
                                t.Span(h.Description);
                            });
                    });
                });

                // BOTTOM METRICS
                col.Item().PaddingTop(8).Row(row =>
                {
                    row.RelativeItem().Border(1).BorderColor(border).Padding(8).Column(box =>
                    {
                        box.Item().Container()
                            .Background(primarySoft).PaddingVertical(3).PaddingHorizontal(6)
                            .Text("Calificativul relației tale").SemiBold().FontSize(11);

                        box.Item().PaddingTop(6).Text(_r.AttachmentStyle.Label + ": " + _r.AttachmentStyleShortText)
                            .FontColor(Colors.Grey.Darken2);

                        box.Item().PaddingTop(4).AlignCenter().Text($"{_r.AverageAspectScore}/10")
                            .FontSize(16).Bold();
                    });

                    row.Spacing(8);

                    row.RelativeItem().Border(1).BorderColor(border).Padding(8).Column(box =>
                    {
                        box.Item().Container()
                            .Background(primarySoft).PaddingVertical(3).PaddingHorizontal(6)
                            .Text("Nivelul toxicității tale").SemiBold().FontSize(11);

                        box.Item().PaddingVertical(10).AlignCenter().Text($"{_r.ToxicityLevel}%")
                            .FontSize(18).Bold().FontColor(Colors.Orange.Darken1);

                        box.Item().AlignCenter().Text("Nu este prea târziu pentru tine!")
                            .FontColor(Colors.Grey.Darken2);
                    });
                });

                // ADVICES
                col.Item().PaddingTop(6).Border(1).BorderColor(border).Padding(8).Column(sec =>
                {
                    sec.Item().Container()
                        .Background(primarySoft).PaddingVertical(3).PaddingHorizontal(6)
                        .Text("Pași imediat următori").SemiBold().FontSize(11);

                    sec.Item().PaddingTop(4).Column(list =>
                    {
                        foreach (var a in _r.AdviceList.Take(3))
                            list.Item().PaddingBottom(1).Container().Text("• " + a).LineHeight(1.15f);
                    });
                });
            });
        });
    }
}
