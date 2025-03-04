using Microsoft.ML;
using Microsoft.ML.Data;

public class CustomerData{
    [LoadColumn(0)]
    public float CustomerID{get;set;}

    [LoadColumn(1)]
    public float AnnualIncome{get;set;}
    [LoadColumn(2)]
    public float SpendingScore{get;set;}
}

public class ClusterPrediction{
    [ColumnName("PredictedLabel")]
    public uint PredictedClusterId {get;set;}
    [ColumnName("Score")]
    public float[] Score{get;set;}

    public float CustomerID{get;set;}
}

public class Program{
    private static readonly string dataPath = Path.Combine(Environment.CurrentDirectory, "customers.csv");

    public static void Main(){
        MLContext mLContext = new MLContext();
        IDataView dataView = mLContext.Data.LoadFromTextFile<CustomerData>(dataPath, separatorChar: ',', hasHeader:true);
        var pipeline = mLContext.Transforms.Concatenate("Features", "AnnualIncome", "SpendingScore").Append(mLContext.Clustering.Trainers.KMeans("Features", numberOfClusters:3));
        var model = pipeline.Fit(dataView);
        var transformedData = model.Transform(dataView);
        var predictions = mLContext.Data.CreateEnumerable<ClusterPrediction>(transformedData,reuseRowObject:false);
        foreach(var prediction in predictions){
            Console.WriteLine($"CustomerID : {prediction.CustomerID}, Predicted Cluster: {prediction.PredictedClusterId}");
        }

    }
}
